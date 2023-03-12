namespace csharp_chess_console; 
using Tabuleiro;
using Xadrez;

class Program
{
    static void Main(string[] args)
    {
        try{
        Tabuleiro tabuleiro = new Tabuleiro(8, 8);
        tabuleiro.ColocarPeca(new Torre(tabuleiro, Cor.Blakc), new Posicao(0, 0));
        tabuleiro.ColocarPeca(new Torre(tabuleiro, Cor.Blakc), new Posicao(0, 9));
        tabuleiro.ColocarPeca(new Rei(tabuleiro, Cor.Blakc), new Posicao(0, 4));

        Tela.ImprimirTabuleiro(tabuleiro);
        }
        catch(TabuleiroException e)
        {
            Console.WriteLine($"{e.Message}");
        }
    }
}