.PHONY: wrunApi wrunClient wrunServer

wrunApi:
	cd OAuth\Api && dotnet watch run

wrunClient:
	cd OAuth\Client && dotnet watch run

wrunServer:
	cd OAuth\Server && dotnet watch run