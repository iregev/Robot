using System.Net.Http;
using System.Threading.Tasks;


public class turn180 : MonoBehaviour
{
    private static readonly HttpClient client = new HttpClient();
    public string URL;
    // Neil: 192.168.0.193
    // Buzz: 192.168.0.187

    public void turn()
    {
        var norm = 0;
        var theta = 179;
        var urlTask = Task.Run(() => Move((int)norm, (int)theta));
    }
    
    public async Task<int[]> Move(int meters, int pivotDegrees)
    {
        var url = $"http://{URL}:8001/WebService2/method";
        // Stop
        int zer = 0;
        var values = new Dictionary<string, string>
        {
            { "distance", zer.ToString() },
            { "angle", zer.ToString()}
        };
        var content = new FormUrlEncodedContent(values);
        var response = await client.PostAsync(url, content);

        // Drive
        var cm = meters * 100;
        values = new Dictionary<string, string>
        {
            { "distance", cm.ToString() },
            { "angle", pivotDegrees.ToString()}
        };
        content = new FormUrlEncodedContent(values);
        response = await client.PostAsync(url, content);
        int[] result = new int[2];
        result[0] = 0;
        result[1] = 0;
        return result;
    }
}
