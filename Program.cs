namespace csharp_chess_console; 
using Tabuleiro;

class Program
{
    static void Main(string[] args)
    {
        Tabuleiro tabuleiro = new Tabuleiro(8, 8);
        
        Tela.ImprimirTabuleiro(tabuleiro);
    }
}