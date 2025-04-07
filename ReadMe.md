# Busquita Bot

A cute music bot to find and get music for telegram

## How Publish

### 1. Install .NET 9 SDK

Before you begin, make sure that the `.NET 9 SDK` is installed on your system. You can download it from the official [.NET website](https://dotnet.microsoft.com/download).

### 2. Compile the Application

Once the SDK is installed, open your terminal and navigate to the root directory of your project. Run the following command to compile the Bot application:

```bash
dotnet publish -c Release -r linux-x64 -o bin Sources/Fembina.Busquita.Bot/Fembina.Busquita.Bot.csproj
```

_This will create the release build for Linux and output the compiled files to the `bin` folder._

### 3. Navigate to the bin Folder

After the publishing process is complete, navigate to the `bin` folder:

```bash
cd bin
```

### 4. Configure the Telegram Token

Before starting the Bot, you need to specify your Telegram token. Open the `config.json` file and add your token in the following format:

```json
{
    "Talkie": {
        "Telegram": {
            "Token": "your_telegram_token_here"
        }
    }
}
```

_Replace `your_telegram_token_here` with your actual Telegram Bot token._

### 5. Run the Busquita Bot

Now, you can run the Bot application. In the bin directory, execute the following command:

```bash
./Busquita
```

_The Bot should now be up and running, connected to Telegram._

## License

This project is licensed under the [GNU General Public License v3.0](License.md).
