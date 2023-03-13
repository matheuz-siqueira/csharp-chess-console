namespace csharp_chess_console; 
using Tabuleiro;
using Xadrez;

class Program
{
    static void Main(string[] args)
    {
        try{
            Partida partida = new Partida();

            while(!partida.Terminada)
            {
                Console.Clear();
                Tela.ImprimirTabuleiro(partida.Tabuleiro);
                Console.WriteLine();

                Console.Write("Origem: ");
                Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();

                bool[,] posicoesPossiveis = partida.Tabuleiro.Peca(origem).MovimentosPossiveis();

                Console.Clear();
                Tela.ImprimirTabuleiro(partida.Tabuleiro, posicoesPossiveis);

                Console.Write("Destino: ");
                Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();

                partida.ExecutaMovimento(origem, destino);
            }
            
        }
        catch(TabuleiroException e)
        {
            Console.WriteLine($"{e.Message}");
        }
        catch(Exception)
        {
            Console.WriteLine("Comando não interpretado.");
        }
    }
}