namespace ParameterDuplicator.Examples.InputFiles;
using System.Threading.Tasks;

class DataService
{
    public async Task FetchDataAsync(int id)
    {
        await Task.Delay(1000);
    }

    private void LogError(string error)
    {
        System.Console.WriteLine(error);
    }

    public void Reset() { }
}