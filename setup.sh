cd libopenmetaverse
./runprebuild.sh
cd ..
nuget install -OutputDirectory packages SimpleBot/packages.config
nuget install -OutputDirectory packages SimpleBot.Tests/packages.config