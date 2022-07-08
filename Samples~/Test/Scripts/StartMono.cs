using Google.Protobuf;
using ILRuntime.Runtime.Enviorment;
using System.IO;
using UnityEngine;

/// <summary>
/// 测试环境最开始执行的代码
/// 用于启动ilruntime
/// 
/// 提示: 仅仅是为了最简单的启动ilruntime, 真实项目中还要做其他很多事情, 但是与本项目无关, 因此省略
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
    /// 必须要添加的代码
    /// 
    /// 这个函数里面的是必须要添加的代码, 但是具体添加到什么地方由自己的ilruntime项目架构决定
    /// </summary>
    private void MustAdd()
    {
        appdomain.DelegateManager.RegisterFunctionDelegate<Google.Protobuf.ProtobufIMessageAdaptor.Adaptor>();

        appdomain.RegisterCrossBindingAdaptor(new ProtobufIMessageAdaptor());
    }
}
