using System.Threading.Tasks;

namespace MGR.CommandLineParser
{
    /// <summary>
    ///
    /// </summary>
    public interface ICommandObject
    {        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        Task<int> ExecuteAsync();
    }
}
