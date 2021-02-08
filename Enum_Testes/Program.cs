using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Enum_Testes
{
    public enum Estados
    {
        [Description("SP")]
        SAO_PAULO,
        [Description("RJ")]
        RIO_DE_JANEIRO,
        [Description("ES")]
        ESPIRITO_SANTO,
        [Description("MG")]
        MINAS_GERAIS,
        [Description("PR")]
        PARANA
    }

    public static class Program
    {
        public static void Main(string[] args)
        {
            //string Sigla = Estados.SAO_PAULO.ToDescriptions();
            //Console.WriteLine("Sigla: " + Sigla);

            //var estado2 = ("SP").ToValueFromDescription<Estados>();
            Console.Write("Entre com a sigla do estado: ");
            string sigla = Console.ReadLine();
            bool retorno = RetornaEstado(sigla);

            if (retorno)
                Console.WriteLine("Existe o estado: " + sigla);
            else
                Console.WriteLine("Não existe o estado: " + sigla);

            Console.ReadKey();
        }

        public static bool RetornaEstado(string sigla)
        {
            bool retorno = false;

            Estados enumestado = sigla.ToUpper().ToValueFromDescription<Estados>();

            switch (enumestado)
            {
                case Estados.SAO_PAULO:
                retorno = true;
                break;
                case Estados.PARANA:
                retorno = true;
                break;
                case Estados.MINAS_GERAIS:
                retorno = true;
                break;
                case Estados.RIO_DE_JANEIRO:
                retorno = true;
                break;
                case Estados.ESPIRITO_SANTO:
                retorno = true;
                break;
            }
            return retorno;
        }

        public static string ToDescriptions(this Enum Enum)
        {
            return GetEnumDescription(Enum);
        }

        public static T ToValueFromDescription<T>(this string strEnum)
        {
            return GetValueFromDescription<T>(strEnum);
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }


        public static T GetValueFromDescriptionXXX<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            return default(T);
        }

        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();

            FieldInfo[] fields = type.GetFields();
            var field = fields.SelectMany(f => f
                              .GetCustomAttributes(typeof(DescriptionAttribute), false), (f, a) => new { Field = f, Att = a })
                              .Where(a => ((DescriptionAttribute)a.Att).Description == description)
                              .SingleOrDefault();

            return field == null ? default : (T)field.Field.GetRawConstantValue();

            // return field == null ? "" : field.Field.Name.ToString();


            /*
            var field = fields.SelectMany(f => f.GetCustomAttributes(typeof(DescriptionAttribute), false),
                                         (f, a) => new { Field = f, Att = a })
                            .Where(a => ((DescriptionAttribute)a.Att)
                                .Description == description).SingleOrDefault();
            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
            */
        }
    }
}
