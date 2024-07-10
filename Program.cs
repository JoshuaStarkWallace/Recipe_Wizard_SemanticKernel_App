using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace SemanticKernelApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create a semantic kernel
            var builder = Kernel.CreateBuilder();

            // Using Azure
            builder.AddAzureOpenAIChatCompletion(
                "SemanticKernelTraining",                              // Azure OpenAI Deployment Name
                "https://neu-ncs-openai-008.openai.azure.com/",  // Azure OpenAI Endpoint
                "9ac1ad1cdecb4c2b9ca7df791d4a0418");             // Azure OpenAI Key

            var kernel = builder.Build();

            // Define a prompt template
            var prompt = @"
            Please take the following input ingredient: '{{$input}}'

            Based on this ingredient, please complete the following tasks:

            1. Suggest 5 unique and creative recipes that prominently feature the given ingredient.
            2. For each recipe, provide a brief description of the dish and its key components.
            3. Rate the difficulty level of preparing each recipe on a scale of 1 to 5, where:
            - 1 indicates a very easy recipe that can be made with minimal effort and skill.
            - 3 indicates a recipe with moderate difficulty, requiring some cooking experience.
            - 5 indicates a highly challenging recipe that demands advanced culinary skills and techniques.
            4. Present the recipes in a clear and concise format, making it easy for users to understand and follow.
            5. Keep responding untill all 5 recipes are suggested.
            ";

            var summarize = kernel.CreateFunctionFromPrompt(prompt, executionSettings: new OpenAIPromptExecutionSettings { MaxTokens = 500 });

            Console.Clear();
            Console.WriteLine("\x1b[1m╔════════════════════════════════════════════════════╗\x1b[0m");
            Console.WriteLine("\x1b[1m║            Welcome to the Recipe Wizard!           ║\x1b[0m");
            Console.WriteLine("\x1b[1m╚════════════════════════════════════════════════════╝\x1b[0m");
            Console.WriteLine();
            Console.WriteLine("This intelligent app is designed to take your available ingredient and suggest 5 unique and creative recipes that prominently feature the given ingredient. Let's get cooking!");
            Console.WriteLine();
            Console.Write("Enter your ingredient: ");
            var inputText = Console.ReadLine();

            var result = await kernel.InvokeAsync(summarize, new() { ["input"] = inputText });

            Console.WriteLine();
            Console.WriteLine("\x1b[1m╔════════════════════════════════════════════════════╗\x1b[0m");
            Console.WriteLine("\x1b[1m║                 Recipe Suggestions                 ║\x1b[0m");
            Console.WriteLine("\x1b[1m╚════════════════════════════════════════════════════╝\x1b[0m");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(result);
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("I hope you found these recipe suggestions helpful! Feel free to run the app again with a different ingredient to explore more culinary possibilities.");
            Console.WriteLine();
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }
}

