using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

// https://stackoverflow.com/questions/56067810/how-do-i-get-the-position-of-an-active-mrtk-pointer
// https://stackoverflow.com/questions/56767566/how-to-always-get-the-tip-of-a-raycast
public class handray : MonoBehaviour, IMixedRealityPointerHandler
{
    public InputSourceType sourceType = InputSourceType.Hand;
    private static readonly HttpClient client = new HttpClient();
    public string URL;

    // Start is called before the first frame update
    void OnEnable()
    {
        CoreServices.InputSystem.Register(gameObject);

    }

    private void OnDisable()
    {
        if (CoreServices.InputSystem != null)
        {
            CoreServices.InputSystem.Unregister(gameObject);
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
        var startPoint = p.Position;
        var endPoint = p.Result.Details.Point;
        // Norm
        var proj = endPoint - startPoint;
        proj.y = 0;
        var norm=proj.magnitude;
        var theta = Vector3.Angle(new Vector3(endPoint.x, 0, endPoint.z), new Vector3(startPoint.x, 0, startPoint.z));
        Debug.Log(startPoint);
        Debug.Log(endPoint);
        Debug.Log(startPoint - endPoint);
        Debug.Log(norm);
        Debug.Log(theta);
    }

    // HTTP POST
    public class Location
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public async Task<int[]> Move(int meters, int pivotDegrees)
    {
        var url = $"http://{URL}:8001/WebService1/move_webservice";
        //Console.WriteLine(url);
        var cm = meters * 100;
        var values = new Dictionary<string, string>
                {
                    { "distance", cm.ToString() },
                    { "angle", pivotDegrees.ToString()}
                };
        var content = new FormUrlEncodedContent(values);
        var response = await client.PostAsync(url, content);
        var responseString = await response.Content.ReadAsStringAsync();
        Location location =
                        JsonSerializer.Deserialize<Location>(responseString);
        /*
        JObject rss = JObject.Parse(responseString);
        string rssX = (string)rss["x"];
        string rssY = (string)rss["x"];
        */
        int[] result = new int[2];
        //            result[0] = Int16.Parse(rssX);
        //          result[1] = Int16.Parse(rssY);
        result[0] = location.x;
        result[1] = location.y;
        return result;

    }







}

