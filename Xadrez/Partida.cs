namespace Xadrez;
using Tabuleiro;

public class Partida
{
    public Tabuleiro Tabuleiro { get; private set; }
    private int Turno;
    private Cor JogadorAtual;

    public bool Terminada { get; private set; } 

    public Partida()
    {
        Tabuleiro = new Tabuleiro(8,8);
        Turno = 1; 
        JogadorAtual = Cor.White;
        ColocarPecas();
    }

    public void ExecutaMovimento(Posicao origem, Posicao destino)
    {
        Peca peca = Tabuleiro.RetirarPeca(origem);
        
        Peca pecaCapturada = Tabuleiro.RetirarPeca(destino);
        Tabuleiro.ColocarPeca(peca, destino);
    }

    private void ColocarPecas()
    {
        Tabuleiro.ColocarPeca(new Torre(Tabuleiro, Cor.Black), new PosicaoXadrez('a', 8).ToPosicao());
        Tabuleiro.ColocarPeca(new Torre(Tabuleiro, Cor.Black), new PosicaoXadrez('h', 8).ToPosicao());
        Tabuleiro.ColocarPeca(new Rei(Tabuleiro, Cor.Black), new PosicaoXadrez('e', 8).ToPosicao());

        Tabuleiro.ColocarPeca(new Torre(Tabuleiro, Cor.White), new PosicaoXadrez('a', 1).ToPosicao());
        Tabuleiro.ColocarPeca(new Torre(Tabuleiro, Cor.White), new PosicaoXadrez('h', 1).ToPosicao());
        Tabuleiro.ColocarPeca(new Rei(Tabuleiro, Cor.White), new PosicaoXadrez('d', 1).ToPosicao());
    }   
}
