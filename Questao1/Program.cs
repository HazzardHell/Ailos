using System;
using System.Globalization;
using System.Threading;

namespace Questao1 {
    class Program {
        static void Main(string[] args) {
            //Boas vindas
            ContaBancaria conta;
            Console.WriteLine("Seja bem vindo ao Core Bank!\n\nAperte ENTER para iniciar");
            Console.ReadLine();
            
            Console.Clear();

            Console.WriteLine("Só um momento enquanto verificamos o plugin de segurança");
            Thread.Sleep(1000);
            Console.Clear();
            Console.WriteLine("Só um momento enquanto verificamos o plugin de segurança.");
            Thread.Sleep(1000);
            Console.Clear();
            Console.WriteLine("Só um momento enquanto verificamos o plugin de segurança..");
            Thread.Sleep(1000);
            Console.Clear();
            Console.WriteLine("Só um momento enquanto verificamos o plugin de segurança...");
            Thread.Sleep(1000);
            Console.Clear();
            Console.WriteLine("Segurança - OK!");
            Thread.Sleep(1000);
            Console.Clear();

            Console.Write("Entre o número da conta: ");
            int numero = int.Parse(Console.ReadLine());
            Console.Write("Entre o titular da conta: ");
            string titular = Console.ReadLine();
            Console.Write("Haverá depósito inicial (s/n)? ");
            char resp = char.Parse(Console.ReadLine());

            if (resp == 's' || resp == 'S') {
                Console.Write("Entre o valor de depósito inicial: ");
                double depositoInicial = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                conta = new ContaBancaria(numero, titular, depositoInicial);
            }
            else {
                conta = new ContaBancaria(numero, titular);
            }

            Console.WriteLine();
            Console.WriteLine("Dados da conta:");
            Console.WriteLine(conta);

            //Fiz pequenos ajustes em nome da usabilidade mas mantive as ações chave descritras na atividade
            Console.WriteLine();
            Console.WriteLine("Digite um numero correspondente a ação desejada:\n");            
            Console.WriteLine("1 - Depósito");
            Console.WriteLine("2 - Saque");
            Console.WriteLine("3 - Sair");
            int opcao = int.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);


            if (opcao == 1)
            {
                Console.WriteLine();
                Console.Write("Entre um valor para depósito: ");
                double quantia = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                conta.Deposito(quantia);
                Console.WriteLine("Dados da conta atualizados:");
                Console.WriteLine(conta);

            }
            else if(opcao == 2)
            {
                Console.WriteLine();
                Console.Write("Entre um valor para saque: ");
                double quantia = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                conta.Saque(quantia);
                Console.WriteLine("Dados da conta atualizados:");
                Console.WriteLine(conta);

            }
            else
            {
                Console.WriteLine();
                Console.Write("Realizando Logout!");
                
            }




            /* Output expected:
            Exemplo 1:

            Entre o número da conta: 5447
            Entre o titular da conta: Milton Gonçalves
            Haverá depósito inicial(s / n) ? s
            Entre o valor de depósito inicial: 350.00

            Dados da conta:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 350.00

            Entre um valor para depósito: 200
            Dados da conta atualizados:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 550.00

            Entre um valor para saque: 199
            Dados da conta atualizados:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 347.50

            Exemplo 2:
            Entre o número da conta: 5139
            Entre o titular da conta: Elza Soares
            Haverá depósito inicial(s / n) ? n

            Dados da conta:
            Conta 5139, Titular: Elza Soares, Saldo: $ 0.00

            Entre um valor para depósito: 300.00
            Dados da conta atualizados:
            Conta 5139, Titular: Elza Soares, Saldo: $ 300.00

            Entre um valor para saque: 298.00
            Dados da conta atualizados:
            Conta 5139, Titular: Elza Soares, Saldo: $ -1.50
            */
        }
    }
}
