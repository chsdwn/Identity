.PHONY: wrunClient wrunServer

wrunClient:
	cd OAuth\Client && dotnet watch run

wrunServer:
	cd OAuth\Server && dotnet watch run