using Google.Protobuf.Reflection;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using System;
using System.Collections.Generic;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace Google.Protobuf
{
    public class ProtobufIMessageAdaptor : CrossBindingAdaptor
    {
        public override Type BaseCLRType => typeof(IMessage<ILTypeInstance>);

        public override Type AdaptorType => typeof(Adaptor);

        public override object CreateCLRInstance(AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adaptor(appdomain, instance);
        }

        public class Adaptor : IMessage<Adaptor>, CrossBindingAdaptorType
        {
            private AppDomain appdomain;
            public AppDomain appDomain
            {
                get => appdomain;
            }
            private ILTypeInstance instance;

            public Adaptor()
            {
            }

            public Adaptor(AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance => instance;

            public MessageDescriptor Descriptor
            {
                get
                {
                    return (MessageDescriptor)appdomain.Invoke(instance.Type.FullName, "get_Descriptor", null, null);

                    //if (_Descriptor == null)
                    //{
                    //    _Descriptor = instance.Type.GetMethod("get_Descriptor", 0);
                    //}
                    //return (MessageDescriptor)appDomain.Invoke(_Descriptor, instance);
                }
            }

            CrossBindingFunctionInfo<int> _CalculateSize = new CrossBindingFunctionInfo<int>("CalculateSize");

            public int CalculateSize()
            {
                return _CalculateSize.Invoke(instance);
            }

            IMethod _Clone = null;

            public Adaptor Clone()
            {
                if (_Clone == null)
                {
                    _Clone = instance.Type.GetMethod("Clone", 0);
                }

                return (Adaptor) appdomain.Invoke(_Clone, instance);
            }

            IMethod _Equals = null;

            public bool Equals(Adaptor other)
            {
                if (_Equals == null)
                {
                    _Equals = instance.Type.GetMethod("Equals", 1);
                }

                return (bool) appdomain.Invoke(_Equals, instance, other.instance);
            }

            //IMethod _MergeFrom2 = null;
            //public void MergeFrom(Adaptor message)
            //{
            //    UnityEngine.Debug.LogError("MergeFrom_adaptor");
            //    if (_MergeFrom2 == null)
            //    {
            //        _MergeFrom2 = instance.Type.GetMethod("MergeFrom", new List<IType>() {
            //            message.instance.Type
            //        }, null);
            //    }
            //    appdomain.Invoke(_MergeFrom2, instance, message.instance);
            //}

            public void MergeFrom(Adaptor message)
            {
                // 猜测可能应为参数类型原因, ilruntime没有调用这个函数, 而是直接走的原类型的函数
                // 因此这里不予实现
                throw new NotImplementedException();
            }

            IMethod _MergeFrom = null;

            public void MergeFrom(CodedInputStream input)
            {
                if (_MergeFrom == null)
                {
                    _MergeFrom = instance.Type.GetMethod("MergeFrom", new List<IType>()
                    {
                        appdomain.GetType(typeof(CodedInputStream))
                    }, null);
                }

                appdomain.Invoke(_MergeFrom, instance, input);
            }

            CrossBindingMethodInfo<CodedOutputStream> _WriteTo =
                new CrossBindingMethodInfo<CodedOutputStream>("WriteTo");

            public void WriteTo(CodedOutputStream output)
            {
                _WriteTo.Invoke(instance, output);
            }

            CrossBindingFunctionInfo<string> _ToString = new CrossBindingFunctionInfo<string>("ToString");

            public override string ToString()
            {
                return _ToString.Invoke(instance);
            }
        }
    }
}