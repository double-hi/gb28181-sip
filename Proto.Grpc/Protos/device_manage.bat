

..\..\packages\Grpc.Tools.1.8.0\tools\windows_x64\protoc.exe -I../protos --csharp_out ../ --grpc_out ../ --plugin=protoc-gen-grpc=..\..\packages\Grpc.Tools.1.8.0\tools\windows_x64\grpc_csharp_plugin.exe ../protos/device_manage.proto