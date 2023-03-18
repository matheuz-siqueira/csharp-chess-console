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

    public Peca VulneravelEnPassant { get; private set; }
    public Partida()
    {
        Tabuleiro = new Tabuleiro(8, 8);
        Turno = 1;
        JogadorAtual = Cor.Branca;
        Xeque = false;
        VulneravelEnPassant = null;
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

        //jogada especial en passant 

        if (peca is Peao)
        {
            if (origem.Coluna != destino.Coluna && pecaCapturada == null)
            {
                Posicao posP;
                if (peca.Cor == Cor.Branca)
                {
                    posP = new Posicao(destino.Linha + 1, destino.Coluna);
                }
                else
                {
                    posP = new Posicao(destino.Linha - 1, destino.Coluna);
                }

                pecaCapturada = Tabuleiro.RetirarPeca(posP);
                Capturadas.Add(pecaCapturada);
            }
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

        //jogada especial en passant 

        if (p is Peao)
        {
            if (origem.Coluna != destino.Coluna && pecaCapturada == VulneravelEnPassant)
            {
                Peca peao = Tabuleiro.RetirarPeca(destino);
                Posicao posP;
                if (p.Cor == Cor.Branca)
                {
                    posP = new Posicao(3, destino.Coluna);
                }
                else
                {
                    posP = new Posicao(4, destino.Coluna);
                }
                Tabuleiro.ColocarPeca(peao, posP);
            }
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

        Peca p = Tabuleiro.Peca(destino);

        //jogada especial en passant 

        if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2))
        {
            VulneravelEnPassant = p;
        }
        else
        {
            VulneravelEnPassant = null;
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
        ColocarNovaPeca('a', 1, new Torre(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('b', 1, new Cavalo(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('c', 1, new Bispo(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('d', 1, new Dama(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('e', 1, new Rei(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('f', 1, new Bispo(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('g', 1, new Cavalo(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('h', 1, new Torre(Tabuleiro, Cor.Branca));
        ColocarNovaPeca('a', 2, new Peao(Tabuleiro, Cor.Branca, this));
        ColocarNovaPeca('b', 2, new Peao(Tabuleiro, Cor.Branca, this));
        ColocarNovaPeca('c', 2, new Peao(Tabuleiro, Cor.Branca, this));
        ColocarNovaPeca('d', 2, new Peao(Tabuleiro, Cor.Branca, this));
        ColocarNovaPeca('e', 2, new Peao(Tabuleiro, Cor.Branca, this));
        ColocarNovaPeca('f', 2, new Peao(Tabuleiro, Cor.Branca, this));
        ColocarNovaPeca('g', 2, new Peao(Tabuleiro, Cor.Branca, this));
        ColocarNovaPeca('h', 2, new Peao(Tabuleiro, Cor.Branca, this));



        ColocarNovaPeca('a', 8, new Torre(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('b', 8, new Cavalo(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('c', 8, new Bispo(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('d', 8, new Rei(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('e', 8, new Dama(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('f', 8, new Bispo(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('g', 8, new Cavalo(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('h', 8, new Torre(Tabuleiro, Cor.Preta));
        ColocarNovaPeca('a', 7, new Peao(Tabuleiro, Cor.Preta, this));
        ColocarNovaPeca('b', 7, new Peao(Tabuleiro, Cor.Preta, this));
        ColocarNovaPeca('c', 7, new Peao(Tabuleiro, Cor.Preta, this));
        ColocarNovaPeca('d', 7, new Peao(Tabuleiro, Cor.Preta, this));
        ColocarNovaPeca('e', 7, new Peao(Tabuleiro, Cor.Preta, this));
        ColocarNovaPeca('f', 7, new Peao(Tabuleiro, Cor.Preta, this));
        ColocarNovaPeca('g', 7, new Peao(Tabuleiro, Cor.Preta, this));
        ColocarNovaPeca('h', 7, new Peao(Tabuleiro, Cor.Preta, this));

    }
}
