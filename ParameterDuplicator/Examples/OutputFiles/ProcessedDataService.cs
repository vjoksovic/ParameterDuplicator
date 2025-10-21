namespace ParameterDuplicator.Examples.InputFiles;
using System.Threading.Tasks;

class ProcessedDataService{
    public async Task FetchDataAsync(int id,int id2)
    {
        await Task.Delay(1000);
    }

    private void LogError(string error,string error2)
    {
        System.Console.WriteLine(error);
    }

    public void Reset() { }
}