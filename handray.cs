using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine.UI;
//using Newtonsoft.Json.Linq;



// https://stackoverflow.com/questions/56067810/how-do-i-get-the-position-of-an-active-mrtk-pointer
// https://stackoverflow.com/questions/56767566/how-to-always-get-the-tip-of-a-raycast
public class handray : MonoBehaviour, IMixedRealityPointerHandler
{
    //public GameObject spawnObject;



    public InputSourceType sourceType = InputSourceType.Hand;
    private static readonly HttpClient client = new HttpClient();
    public string URL;
    // Neil: 192.168.0.193
    // Buzz: 192.168.0.187


    // Start is called before the first frame update
    void OnEnable()
    {
        //CoreServices.InputSystem.Register(gameObject);
        CoreServices.InputSystem.RegisterHandler<IMixedRealityPointerHandler>(this);
    }



    private void OnDisable()
    {
        if (CoreServices.InputSystem != null)
        {
            CoreServices.InputSystem.UnregisterHandler<IMixedRealityPointerHandler>(this);
        }
    }



    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        if (eventData.InputSource.SourceType == sourceType)
        {



        }
    }



    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
    }



    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
    }



    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        var p = eventData.Pointer;
        if (p is IMixedRealityNearPointer)
        {
            // Ignore near pointers, we only want the rays
            return;
        }
        var startPoint = p.Position;
        var endPoint = p.Result.Details.Point;
        // Norm
        var proj = endPoint - startPoint;
        proj.y = 0;
        var norm = proj.magnitude;
        var theta = Vector3.Angle(new Vector3(endPoint.x, 0, endPoint.z), new Vector3(startPoint.x, 0, startPoint.z));
        //Debug.Log(startPoint);
        //Debug.Log(endPoint);
        //Debug.Log(startPoint - endPoint);
        if (theta <= 180)
        {
            theta -= 10;
        }
        else
        {
            theta = 360 - theta;
            theta *= -1;
        }

        if (norm > 5)
        {
            norm = 5;
        }
        Debug.Log($"{norm}, {theta}");

        var urlTask = Task.Run(() => Move((int)norm, (int)theta));
        /*
        var urlResult = urlTask.Result;
        Debug.Log(urlResult[0]);
        Debug.Log(urlResult[1]);
        */
    }



    // HTTP POST
    public class Location
    {
        public int x { get; set; }
        public int y { get; set; }
    }



    public async Task<int[]> Move(int meters, int pivotDegrees)
    {
        var url = $"http://{URL}:8001/WebService2/method";
        // Stop
        int zer = 0;
        var values = new Dictionary<string, string>
        {
            { "distance",zer.ToString() },
            { "angle", zer.ToString()}
        };
        var content = new FormUrlEncodedContent(values);
        var response = await client.PostAsync(url, content);

        // Drive
        //Console.WriteLine(url);
        var cm = meters * 100;
        //var url = "http://192.168.0.187:8001/WebService2/method";
        //Debug.Log(url);
        values = new Dictionary<string, string>
        {
            { "distance", cm.ToString() },
            { "angle", pivotDegrees.ToString()}
        };
        content = new FormUrlEncodedContent(values);
        response = await client.PostAsync(url, content);
        //var responseString = await response.Content.ReadAsStringAsync();
        /* Location location;
        location = JsonUtility.FromJson<Location>(responseString);
        //Location location =
        // JsonSerializer.Deserialize<Location>(JsonReader(responseString));
        // JObject rss = JObject.Parse(responseString);
        // string rssX = (string)rss["x"];
        // string rssY = (string)rss["x"];
        */
        int[] result = new int[2];
        // result[0] = System.Int16.Parse(rssX);
        // result[1] = System.Int16.Parse(rssY);
        result[0] = 0;
        result[1] = 0;
        return result;
    }
}
