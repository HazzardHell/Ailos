using System;
using System.Globalization;

namespace Questao1
{
    class ContaBancaria {
        public int Numero { get; private set; }
        public string Titular { get; private set; }
        public double Saldo { get; private set; }

        public ContaBancaria(int numero, string titular)
        {
            Numero = numero;
            Titular = titular;
            Saldo = 0.0;
        }

        public ContaBancaria(int numero, string titular, double depositoInicial) : this(numero, titular)
        {
            Deposito(depositoInicial);
        }

        public void Deposito(double quantia)
        {
            if (quantia <= 0)
            {
                Console.WriteLine("O valor do depósito deve ser positivo.");
            }
            else
            {
                Saldo += quantia;
            }
        }

        public void Saque(double quantia)
        {
            if (quantia <= 0)
            {
                Console.WriteLine("O valor do saque deve ser positivo.");
            }
            else if (quantia > Saldo)
            {
                Console.WriteLine("Saldo insuficiente.");
            }
            else
            {
                Saldo -= quantia;
            }
        }

        public override string ToString()
        {
            return $"Conta {Numero}, Titular: {Titular}, Saldo: ${Saldo.ToString("F2", CultureInfo.InvariantCulture)}";
        }
    }

}

