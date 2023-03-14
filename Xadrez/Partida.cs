namespace Xadrez;
using Tabuleiro;

public class Partida
{
    public Tabuleiro Tabuleiro { get; private set; }
    public int Turno { get; private set; }
    public Cor JogadorAtual { get; private set; }

    public bool Terminada { get; private set; }
    private HashSet<Peca> Pecas;
    private HashSet<Peca> Capturadas;
    public bool Xeque { get; private set; }
    public Partida()
    {
        Tabuleiro = new Tabuleiro(8, 8);
        Turno = 1;
        JogadorAtual = Cor.Branca;
        Xeque = false;
        Pecas = new HashSet<Peca>();
        Capturadas = new HashSet<Peca>();
        ColocarPecas();
    }

    public Peca ExecutaMovimento(Posicao origem, Posicao destino)
    {
        Peca peca = Tabuleiro.RetirarPeca(origem);
        peca.IncrementarMovimentos();
        Peca pecaCapturada = Tabuleiro.RetirarPeca(destino);
        Tabuleiro.ColocarPeca(peca, destino);

        if (pecaCapturada != null)
        {
            Capturadas.Add(pecaCapturada);
        }

        return pecaCapturada;
    }
    public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
    {
        Peca p = Tabuleiro.RetirarPeca(destino);
        p.DecrementarMovimentos();
        if (pecaCapturada != null)
        {
            Tabuleiro.ColocarPeca(pecaCapturada, destino);
            Capturadas.Remove(pecaCapturada);
        }

        Tabuleiro.ColocarPeca(p, origem);
    }
    public void RealizaJogada(Posicao origem, Posicao destino)
    {
        Peca pecaCapturada = ExecutaMovimento(origem, destino);
        if (EstaEmCheque(JogadorAtual))
        {
            DesfazMovimento(origem, destino, pecaCapturada);
            throw new TabuleiroException("Você não pode se colocar em xeque!");
        }

        if (EstaEmCheque(Adversaria(JogadorAtual)))
        {
            Xeque = true;
        }
        else
        {
            Xeque = false;
        }

        if (TesteXequemate(Adversaria(JogadorAtual)))
        {
            Terminada = true;
        }
        else
        {
            Turno++;
            MudaJogador();
        }
    }

    public void ValidarPosicaoOrigem(Posicao pos)
    {
        if (Tabuleiro.Peca(pos) == null)
        {
            throw new TabuleiroException("Não existe peça na coordenada informada!");
        }
        if (JogadorAtual != Tabuleiro.Peca(pos).Cor)
        {
            throw new TabuleiroException("A peça de origem escolhida não é sua!");
        }
        if (!Tabuleiro.Peca(pos).ExisteMovimentosPossiveis())
        {
            throw new TabuleiroException("Peça escolhida está bloqueada!");
        }
    }

    public void ValidarPosicaoDestino(Posicao origem, Posicao destino)
    {
        if (!Tabuleiro.Peca(origem).MovimentoPossivel(destino))
        {
            throw new TabuleiroException("Posição de destino inválida!");
        }
    }

    private void MudaJogador()
    {
        if (JogadorAtual == Cor.Branca)
        {
            JogadorAtual = Cor.Preta;
        }
        else
        {
            JogadorAtual = Cor.Branca;
        }
    }

    public HashSet<Peca> PecasCapturadas(Cor cor)
    {
        HashSet<Peca> aux = new HashSet<Peca>();
        foreach (Peca x in Capturadas)
        {
            if (x.Cor == cor)
            {
                aux.Add(x);
            }
        }

        return aux;
    }

    public HashSet<Peca> PecasEmJogo(Cor cor)
    {
        HashSet<Peca> aux = new HashSet<Peca>();
        foreach (Peca x in Pecas)
        {
            if (x.Cor == cor)
            {
                aux.Add(x);
            }
        }
        aux.ExceptWith(PecasCapturadas(cor));
        return aux;
    }

    private Cor Adversaria(Cor cor)
    {
        if (cor == Cor.Branca)
        {
            return Cor.Preta;
        }
        else
        {
            return Cor.Branca;
        }
    }

    private Peca Rei(Cor cor)
    {
        foreach (Peca x in PecasEmJogo(cor))
        {
            if (x is Rei)
            {
                return x;
            }
        }
        return null;
    }

    public bool EstaEmCheque(Cor cor)
    {
        Peca R = Rei(cor);
        if (R == null)
        {
            throw new TabuleiroException($"Não tem rei da cor {cor} no tabuleiro");
        }

        foreach (Peca x in PecasEmJogo(Adversaria(cor)))
        {
            bool[,] mat = x.MovimentosPossiveis();
            if (mat[R.Posicao.Linha, R.Posicao.Coluna])
            {
                return true;
            }
        }
        return false;
    }

    public bool TesteXequemate(Cor cor)
    {
        if (!EstaEmCheque(cor))
        {
            return false;
        }

        foreach (Peca x in PecasEmJogo(cor))
        {
            bool[,] mat = x.MovimentosPossiveis();
            for (int i = 0; i < Tabuleiro.Linhas; i++)
            {
                for (int j = 0; j < Tabuleiro.Colunas; j++)
                {
                    if (mat[i, j])
                    {
                        Posicao origem = x.Posicao;
                        Posicao destino = new Posicao(i, j);
                        Peca pecaCapturada = ExecutaMovimento(origem, destino);
                        bool testeXeque = EstaEmCheque(cor);
                        DesfazMovimento(origem, destino, pecaCapturada);
                        if (!testeXeque)
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }
    public void ColocarNovaPeca(char coluna, int linha, Peca peca)
    {
        Tabuleiro.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
        Pecas.Add(peca);
    }

    private void ColocarPecas()
    {
        ColocarNovaPeca('b', 8, new Torre(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('a', 8, new Rei(Tabuleiro, Cor.Preta));

        ColocarNovaPeca('c', 1, new Torre(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('h', 7, new Torre(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('d', 1, new Rei(Tabuleiro, Cor.Branca));
    }
}
