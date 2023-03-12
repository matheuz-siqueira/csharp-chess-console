namespace csharp_chess_console; 
using Tabuleiro;
using Xadrez;

class Program
{
    static void Main(string[] args)
    {
        PosicaoXadrez posicao = new PosicaoXadrez('c',7);

        Console.WriteLine(posicao);
        Console.WriteLine(posicao.ToPosicao());
    }
}