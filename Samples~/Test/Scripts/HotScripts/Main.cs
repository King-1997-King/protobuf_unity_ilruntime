using Google.Protobuf;
using UnityEngine;

/// <summary>
/// �������������ilruntime�е�
/// </summary>
public class Main
{
    public static byte[] ToByte(IMessage msg)
    {
        return msg.ToByteArray();
    }

    public static T ToObj<T>(byte[] bytes) where T : IMessage, new()
    {
        T obj = new T();
        obj.MergeFrom(bytes);
        return obj;
    }

    /// <summary>
    /// һ���ȸ���������￪ʼ
    /// </summary>
    public static void Start()
    {
        GxTest.gx_data data = new GxTest.gx_data();

        data.ScString = "sdf151";
        data.ScInt32 = 52;
        data.ScFloat = 43.6f;
        data.Enm = GxTest.mjEnum.Local;

        global::GxTest.majun majun = new GxTest.majun();
        majun.F.Add(1);
        majun.F.Add(2);
        majun.F.Add(3);
        majun.M.Add(1, new GxTest.val() { A = 36, B = "ddd" });
        data.Mj.Add(majun);

        byte[] bytes = ToByte(data);
        GxTest.gx_data data2 = ToObj<GxTest.gx_data>(bytes);

        Debug.LogError(data2);
    }
}
