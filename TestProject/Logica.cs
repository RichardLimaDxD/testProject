using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TestProject
{
    public class Logica
    {
        /// <summary>
        /// Metodo recebe um numero em texto usando separador . como separador de milhar e , como separador decimal
        /// </summary>
        /// <param name="numeroString"></param>
        /// <returns></returns>
        internal decimal ConverteStringParaDecimal(string numeroString)
        {
            if (decimal.TryParse(numeroString, NumberStyles.Currency, CultureInfo.GetCultureInfo("pt-BR"), out decimal result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Metodo recebe uma data em texto no formato dd/MM/yyyy e retorna a data convertida
        /// </summary>
        /// <param name="dataString"></param>
        /// <returns></returns>
        internal DateTime ConverteStringParaData(string dataString)
        {
            if (DateTime.TryParseExact(dataString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Vendedor Gustavo
        /// Código Produto	quantidade    valor total          Data venda
        /// ARA-1012        17 UN          R$ 3.642,17              08/04/2021
        /// </summary>
        /// <param name="produtosString"></param>
        /// <returns></returns>
        /// Dica, pode ser usado regex para separar a string pro padrões.

        internal List<VendaTO> ConverteStringParaVendas(string produtosString)
        {
            List<VendaTO> vendas = new List<VendaTO>();
            string[] linhas = produtosString.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            string vendedorAtual = "";

            foreach (var linha in linhas)
            {
                if (linha.Trim().StartsWith("Vendedor"))
                {
                    vendedorAtual = linha.Trim().Split(' ')[1];
                }
                else
                {
                    var valores = Regex.Split(linha, @"\s+").Where(s => !string.IsNullOrEmpty(s)).ToArray();
                    if (valores.Length == 6 && valores[0] != "Código")
                    {
                        var valorString = valores[4].Replace("R$", "").Replace(",", "").Trim();
                        var valor = decimal.Parse(valorString, NumberStyles.Currency, CultureInfo.CreateSpecificCulture("pt-BR")) / 100;

                        var venda = new VendaTO
                        {
                            Vendedor = vendedorAtual,
                            Codigo = valores[0],
                            Quantidade = int.Parse(valores[1]),
                            Valor = valor,
                            Data = DateTime.ParseExact(valores[5].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                        };

                        vendas.Add(venda);
                    }
                }
            }

            return vendas;
        }

        internal int ConvertStringParaInt(string value)
        {
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
    
}
}
