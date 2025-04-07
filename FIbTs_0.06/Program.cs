using System;
using System.IO;
using System.Numerics;

namespace FIbTs_0._06
{
    class Program
    {
        static void Main(string[] args)
        {
            bool keepRunning = true;

            // Prompt the user to specify the output file
            Console.WriteLine("Enter the output file name (or press Enter to use 'output.txt'):");
            string fileName = Console.ReadLine()?.Trim();

            // Use a default file name if none was entered
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = "output.txt";
            }

            // Attempt to ensure that the provided file can be created or accessed
            try
            {
                using StreamWriter writer = new StreamWriter(fileName, append: true);
                Console.WriteLine($"Output will be saved to '{fileName}'.");
                
                // Call the CheckForSubstring method
                Program program = new Program();
                program.CheckForSubstring(writer, ref keepRunning);

                Console.WriteLine($"Execution completed. Output saved to '{fileName}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while accessing the file '{fileName}': {ex.Message}");
            }
        }

        public void CheckForSubstring(StreamWriter writer, ref bool keepRunning)
        {
            BigInteger a = 0, b = 1;
            int iteration = 1; // Tracks Fibonacci sequence index
            int occurrenceCount = 0; // Tracks total occurrences across all iterations
            const string targetSequence = "003706";

            Console.WriteLine("Checking Fibonacci numbers for sequence '003706'...");
            Console.WriteLine("Writing results to the specified file...");

            while (keepRunning)
            {
                try
                {
                    // Generate the next Fibonacci number
                    BigInteger c = a + b;
                    string fibString = c.ToString();

                    // Search for all occurrences of the target sequence within the current number
                    int startIndex = 0;

                    while (true)
                    {
                        // Search for the target sequence starting from the current index
                        int index = fibString.IndexOf(targetSequence, startIndex);
                        if (index == -1)
                            break; // No more occurrences found

                        // Ensure there are 7 digits following the target sequence
                        if (index + targetSequence.Length + 7 <= fibString.Length)
                        {
                            // Extract the 7 digits after the target sequence
                            string followingDigits = fibString.Substring(index + targetSequence.Length, 7);

                            // Increment the total occurrences across all iterations
                            occurrenceCount++;

                            // Prepare the output message
                            string output = $"Sequence found! Following numbers: {followingDigits}, Iteration: {iteration}, Found #{occurrenceCount}";
                            
                            // Log to console and file
                            Console.WriteLine(output);
                            writer.WriteLine(output);
                            writer.Flush();
                        }

                        // Move the search index forward to find additional matches
                        startIndex = index + 1;
                    }

                    // Update Fibonacci sequence for the next iteration
                    a = b;
                    b = c;
                    iteration++;

                    // Check if the user wants to stop the program
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(intercept: true).Key;
                        if (key == ConsoleKey.S)
                        {
                            Console.WriteLine("Stopping the program...");
                            keepRunning = false;
                        }
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"File writing error: {ex.Message}");
                    keepRunning = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    keepRunning = false;
                }
            }
        }
    }
}