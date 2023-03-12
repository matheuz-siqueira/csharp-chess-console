namespace csharp_chess_console; 
using Tabuleiro;
using Xadrez;

class Program
{
    static void Main(string[] args)
    {
        try{
        Tabuleiro tabuleiro = new Tabuleiro(8, 8);
        tabuleiro.ColocarPeca(new Torre(tabuleiro, Cor.Black), new Posicao(0, 0));
        tabuleiro.ColocarPeca(new Torre(tabuleiro, Cor.Black), new Posicao(0, 7));
        tabuleiro.ColocarPeca(new Rei(tabuleiro, Cor.Black), new Posicao(0, 4));

        tabuleiro.ColocarPeca(new Torre(tabuleiro, Cor.White), new Posicao(7, 0));
        tabuleiro.ColocarPeca(new Torre(tabuleiro, Cor.White), new Posicao(7, 7));
        tabuleiro.ColocarPeca(new Rei(tabuleiro, Cor.White), new Posicao(7, 5));

        Tela.ImprimirTabuleiro(tabuleiro);
        }
        catch(TabuleiroException e)
        {
            Console.WriteLine($"{e.Message}");
        }
    }
}