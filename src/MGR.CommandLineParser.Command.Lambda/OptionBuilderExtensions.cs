using System.ComponentModel.DataAnnotations;

namespace MGR.CommandLineParser.Command.Lambda
{
    public static class OptionBuilderExtensions
    {
        public static OptionBuilder AddValidation<TValidation>(this OptionBuilder optionBuilder)
            where TValidation : ValidationAttribute, new()
        {
            optionBuilder.AddValidation(new TValidation());
            return optionBuilder;
        }
        public static OptionBuilder Required(this OptionBuilder optionBuilder)
        {
            return optionBuilder.AddValidation<RequiredAttribute>();
        }
    }
}