namespace csharp_chess_console; 
using Tabuleiro;
using Xadrez;

class Program
{
    static void Main(string[] args)
    {
        Tabuleiro tabuleiro = new Tabuleiro(8, 8);
        tabuleiro.ColocarPeca(new Torre(tabuleiro, Cor.Blakc), new Posicao(0, 0));
        tabuleiro.ColocarPeca(new Torre(tabuleiro, Cor.Blakc), new Posicao(0, 7));
        tabuleiro.ColocarPeca(new Rei(tabuleiro, Cor.Blakc), new Posicao(0, 4));

        Tela.ImprimirTabuleiro(tabuleiro);
    }
}