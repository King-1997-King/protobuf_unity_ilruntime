# protobuf_unity_ilruntime
可用于unity ilruntime使用的基于官方3.12.4版本的protobuf

可以使用所有的protobuf功能, 未做任何的功能删减

# 使用方式
1. 将Protobuf文件夹放入到主工程里面(非热更工程)
2. 在生成ilruntime appDomain的地方加入代码:  ProtobufIMessageAdaptor.appDomain = appDomain;
3. 在注册委托的地方加入代码:  app.DelegateManager.RegisterFunctionDelegate<Google.Protobuf.ProtobufIMessageAdaptor.Adaptor>();
4. 在注册适配器的地方加入代码:   app.RegisterCrossBindingAdaptor(new ProtobufIMessageAdaptor());
5. 可以愉快并且没有限制的使用了 ~^0^~
