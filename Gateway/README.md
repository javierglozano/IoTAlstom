# ALSTOMeHUB

### Dotnet binding

#### How to add modules

    Drop the compiled module DLL referencing the "Microsoft.Azure.IoT.Gateway.dll" and implementing the interfaces "IGatewayModul" and "IGatewayModuleStart"
	
	Update the json file to include the module, for example:
		{
            "name": "simple_module",
            "loader": {
                "name": "dotnetcore",
                "entrypoint": {

                    "assembly.name": "SimpleModule",
                    "entry.type": "SimpleModule.SimpleModule"
                }
            },
			"args": "This can be a json or just a string"
        }

	Run the exe passing as parameter the updated json file
