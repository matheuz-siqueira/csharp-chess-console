namespace csharp_chess_console;
using Tabuleiro;
using Xadrez;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Partida partida = new Partida();

            while (!partida.Terminada)
            {
                try
                {
                    Console.Clear();
                    Tela.ImprimirPartida(partida);

                    Console.Write("Origem: ");
                    Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();
                    partida.ValidarPosicaoOrigem(origem);

                    bool[,] posicoesPossiveis = partida.Tabuleiro.Peca(origem).MovimentosPossiveis();

                    Console.Clear();
                    Tela.ImprimirTabuleiro(partida.Tabuleiro, posicoesPossiveis);

                    Console.Write("Destino: ");
                    Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();
                    partida.ValidarPosicaoDestino(origem, destino);

                    partida.RealizaJogada(origem, destino);
                }
                catch (TabuleiroException e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
            }
            Console.Clear();
            Tela.ImprimirPartida(partida);

        }
        catch (TabuleiroException e)
        {
            Console.WriteLine($"{e.Message}");
        }
    }
}