using Google.Protobuf;
using ILRuntime.Runtime.Enviorment;
using System.IO;
using UnityEngine;

/// <summary>
/// ���Ի����ʼִ�еĴ���
/// ��������ilruntime
/// 
/// ��ʾ: ������Ϊ����򵥵�����ilruntime, ��ʵ��Ŀ�л�Ҫ�������ܶ�����, �����뱾��Ŀ�޹�, ���ʡ��
/// </summary>
public class StartMono : MonoBehaviour
{
    AppDomain appdomain;

    void Start()
    {
        appdomain = new AppDomain();

        byte[] dll = File.ReadAllBytes("Library/ScriptAssemblies/HotScripts.dll");
        byte[] pdb = File.ReadAllBytes("Library/ScriptAssemblies/HotScripts.pdb");

        System.IO.MemoryStream fs = new MemoryStream(dll);
        System.IO.MemoryStream p = new MemoryStream(pdb);

        appdomain.LoadAssembly(fs, p, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());

        appdomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;

        MustAdd();

        appdomain.Invoke("Main", "Start", null, null);
    }

    /// <summary>
    /// ����Ҫ��ӵĴ���
    /// 
    /// �������������Ǳ���Ҫ��ӵĴ���, ���Ǿ�����ӵ�ʲô�ط����Լ���ilruntime��Ŀ�ܹ�����
    /// </summary>
    private void MustAdd()
    {
        appdomain.DelegateManager.RegisterFunctionDelegate<Google.Protobuf.ProtobufIMessageAdaptor.Adaptor>();

        appdomain.RegisterCrossBindingAdaptor(new ProtobufIMessageAdaptor());
    }
}
