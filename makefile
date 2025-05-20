pack:
	if defined DIR ( \
	    dotnet build -c release -p:PackDir=$(shell for %%A in ("$(DIR)") do @echo %%~fA) \
	) else ( \
	    dotnet build -c release \
	)