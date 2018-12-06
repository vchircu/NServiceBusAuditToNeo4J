namespace Importer
{
    using System.Threading.Tasks;

    using ModelBuilder;

    public interface IModelImporter
    {
        Task ImportAsync(Model model);
    }
}