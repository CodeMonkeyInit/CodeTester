let assert = chai.assert;

let secondsToMs = seconds => seconds * 1000;

let minutesToMs = minutes => secondsToMs(60 * minutes);

describe("Code analysis", function () {
    let languages = Api.supportedLanguages;

    //1 hour
    this.timeout(minutesToMs(10));

    describe("C++ sum with score 100 (stdin/stdout)", function () {
        it("should have analysis result, execution result and mark", async function () {
            this.slow(secondsToMs(60));
            
            // language=C++
            let codeToRun = `
            #include <iostream>
            #include <string>
            #include <vector>
            
            using namespace std;
            
            vector<string> split(const string& str, const string& delim)
            {
                vector<string> tokens;
                size_t prev = 0, pos = 0;
                do
                {
                    pos = str.find(delim, prev);
                    if (pos == string::npos) pos = str.length();
                    string token = str.substr(prev, pos-prev);
                    if (!token.empty()) tokens.push_back(token);
                    prev = pos + delim.length();
                }
                while (pos < str.length() && prev < str.length());
                return tokens;
            }
            
            int main()
            {
              string arguments;
              getline (std::cin, arguments);
              
              auto results = split(arguments, " ");
              
              int sum = 0;
              
              for(auto const& value: results) {
                  int singleValue = stoi(value);
                  sum += singleValue;
              }
              
              cout << sum;
            }`;
            let expectedScore = 100;

            let actualResult = await Api.fetchRequestForSumTask(languages.CPlusPlus, codeToRun);

            assert.isNotNull(actualResult.codeAnalysisResult, "Analysis result is not null");
            assert.isNotNull(actualResult.codeExecutionResult, "Execution result is not null");

            assert.isTrue(actualResult.codeAnalysisResult.isSuccessful, "Analysis result successful");
            assert.isTrue(actualResult.codeExecutionResult.wasSuccessful, "Execution result successful");

            assert(actualResult.score === expectedScore,
                `Actual code score equals to expected, expected: ${expectedScore} actual: ${actualResult.score}`);

        })
    });

    describe("C++ sum with score 100 (input file/output file)", function () {
        it("should have analysis result, execution result and mark", async function () {
            this.slow(secondsToMs(60));
            //language=C++
            let codeToRun = `
            #include <iostream>
            #include <fstream>
            #include <string>
            #include <vector>
            
            using namespace std;
            
            vector<string> split(const string& str, const string& delim)
            {
                vector<string> tokens;
                size_t prev = 0, pos = 0;
                do
                {
                    pos = str.find(delim, prev);
                    if (pos == string::npos) pos = str.length();
                    string token = str.substr(prev, pos-prev);
                    if (!token.empty()) tokens.push_back(token);
                    prev = pos + delim.length();
                }
                while (pos < str.length() && prev < str.length());
                return tokens;
            }
            
            int main()
            {
              string arguments;
              
              ifstream inputFile;
              ofstream outputFile;
               
              inputFile.open("input.txt");
              outputFile.open("output.txt");
              
              getline(inputFile, arguments);
              
              auto results = split(arguments, " ");
              
              int sum = 0;
              
              for(auto const& value: results) {
                  int singleValue = stoi(value);
                  sum += singleValue;
              }
              
              outputFile << sum;
            }`;

            let expectedScore = 100;

            let actualResult = await Api.fetchRequestForSumTask(languages.CPlusPlus, codeToRun);

            assert.isNotNull(actualResult.codeAnalysisResult, "Analysis result is not null");
            assert.isNotNull(actualResult.codeExecutionResult, "Execution result is not null");

            assert.isTrue(actualResult.codeAnalysisResult.isSuccessful, "Analysis result successful");
            assert.isTrue(actualResult.codeExecutionResult.wasSuccessful, "Execution result successful");

            assert(actualResult.score === expectedScore,
                `Actual code score equals to expected, expected: ${expectedScore} actual: ${actualResult.score}`);

        })
    });

    describe("C++ infinite loop", function () {
        it("all test runs should be killed by time limit", async function () {
            this.slow(minutesToMs(60));
            //language=C++
            let codeToRun = `
                int main() 
                { 
                    while (true) {
                      //infinite loop
                    }
                } `;

            let expectedScore = 0;

            let actualResult = await Api.fetchRequestForSumTask(languages.CPlusPlus, codeToRun);

            assert.isNotNull(actualResult.codeAnalysisResult, "Analysis result is not null");
            assert.isNotNull(actualResult.codeExecutionResult, "Execution result is not null");

            assert.isTrue(actualResult.codeAnalysisResult.isSuccessful, "Analysis result successful");
            assert.isTrue(actualResult.codeExecutionResult.wasSuccessful, "Execution result successful");

            assert(actualResult.score === expectedScore,
                `Actual code score equals to expected, expected: ${expectedScore} actual: ${actualResult.score}`);
            
            for (let testRunResult of actualResult.codeExecutionResult.results) {
                assert(testRunResult.executionResult === Api.executionResults.KilledByTimeout, 
                    `All test runs should be killed by timeout current execution result: ${testRunResult.executionResult}`);
            }

        })
    });

    describe("C++ memory limit", function () {
        it("all test runs should be killed by memory limit", async function () {
            this.slow(minutesToMs(30));
            //language=C++
            let codeToRun = `
                int main() 
                { 
                    while (true) 
                      int *a = new int;  // infinite loop memory allocation   
                }`;

            let expectedScore = 0;

            let actualResult = await Api.fetchRequestForSumTask(languages.CPlusPlus, codeToRun);

            assert.isNotNull(actualResult.codeAnalysisResult, "Analysis result is not null");
            assert.isNotNull(actualResult.codeExecutionResult, "Execution result is not null");

            assert.isTrue(actualResult.codeAnalysisResult.isSuccessful, "Analysis result successful");
            assert.isTrue(actualResult.codeExecutionResult.wasSuccessful, "Execution result successful");

            assert(actualResult.score === expectedScore,
                `Actual code score equals to expected, expected: ${expectedScore} actual: ${actualResult.score}`);

            for (let testRunResult of actualResult.codeExecutionResult.results) {
                assert(testRunResult.executionResult === Api.executionResults.KilledByMemoryLimit,
                    `All test runs should be killed by memory limit current execution result: ${testRunResult.executionResult}`);
            }

        })
    });

    describe("Php sum with score 100 (stdin/stdout)", function () {
        it("should have analysis result, execution result and mark", async function () {
            this.slow(secondsToMs(60));
            // language=Php
            let codeToRun = `
                <?php
    
                $arguments = fgets(STDIN);
                
                $numbers = explode(' ', $arguments);
                
                echo array_reduce($numbers, function($prev, $current) {
                    return $prev + $current;
                });`;

            let expectedScore = 100;

            let actualResult = await Api.fetchRequestForSumTask(languages.Php, codeToRun);

            assert.isNotNull(actualResult.codeAnalysisResult, "Analysis result is not null");
            assert.isNotNull(actualResult.codeExecutionResult, "Execution result is not null");

            assert.isTrue(actualResult.codeAnalysisResult.isSuccessful, "Analysis result successful");
            assert.isTrue(actualResult.codeExecutionResult.wasSuccessful, "Execution result successful");

            assert(actualResult.score === expectedScore,
                `Actual code score equals to expected, expected: ${expectedScore} actual: ${actualResult.score}`);

        })
    });

    describe("Php sum with score 100 (input file/output file)", function () {
	this.slow(minutesToMs(1))
        it("should have analysis result, execution result and mark", async function () {
            //language=Php
            let codeToRun = `
            <?php
            
            $file = fopen("input.txt", "r");
            
            $arguments = fgets($file);
            
            $numbers = explode(' ', $arguments);
            
            $result = array_reduce($numbers, function($prev, $current) {
                return $prev + $current;
            });
            
            file_put_contents("output.txt", $result);`;

            let expectedScore = 100;

            let actualResult = await Api.fetchRequestForSumTask(languages.Php, codeToRun);

            assert.isNotNull(actualResult.codeAnalysisResult, "Analysis result is not null");
            assert.isNotNull(actualResult.codeExecutionResult, "Execution result is not null");

            assert.isTrue(actualResult.codeAnalysisResult.isSuccessful, "Analysis result successful");
            assert.isTrue(actualResult.codeExecutionResult.wasSuccessful, "Execution result successful");

            assert(actualResult.score === expectedScore,
                `Actual code score equals to expected, expected: ${expectedScore} actual: ${actualResult.score}`);

        })
    });

    describe("JavaScript sum with score 97 because call to console is a warning (stdin/stdout)", function () {
        it("should have analysis result, execution result and mark", async function () {
            this.slow(secondsToMs(60));
            // language=JavaScript
            let codeToRun = `
const readline = require('readline');

const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout,
  terminal: false,
});

rl.on('line', (line) => {
  const exarguments = line.split(' ').map(arg => +arg);

  const sum = exarguments.reduce((current, next) => current + next);

  console.log(sum);
});
`;

            let expectedScore = 97;

            let actualResult = await Api.fetchRequestForSumTask(languages.Js, codeToRun);

            assert.isNotNull(actualResult.codeAnalysisResult, "Analysis result is not null");
            assert.isNotNull(actualResult.codeExecutionResult, "Execution result is not null");

            assert.isTrue(actualResult.codeAnalysisResult.isSuccessful, "Analysis result successful");
            assert.isTrue(actualResult.codeExecutionResult.wasSuccessful, "Execution result successful");

            assert(actualResult.score === expectedScore,
                `Actual code score equals to expected, expected: ${expectedScore} actual: ${actualResult.score}`);

        })
    });

    describe("JavaScript sum with score 100 (input file/output file)", function () {
        this.slow(secondsToMs(60));
        it("should have analysis result, execution result and mark", async function () {
            //language=JavaScript
            let codeToRun = `
const fs = require('fs');

fs.readFile('input.txt', 'utf8', (err, contents) => {
  const exarguments = contents.split(' ').map(arg => +arg);

  const sum = exarguments.reduce((current, next) => current + next);

  fs.appendFile('output.txt', sum, () => ({}));
});
`;

            let expectedScore = 100;

            let actualResult = await Api.fetchRequestForSumTask(languages.Js, codeToRun);

            assert.isNotNull(actualResult.codeAnalysisResult, "Analysis result is not null");
            assert.isNotNull(actualResult.codeExecutionResult, "Execution result is not null");

            assert.isTrue(actualResult.codeAnalysisResult.isSuccessful, "Analysis result successful");
            assert.isTrue(actualResult.codeExecutionResult.wasSuccessful, "Execution result successful");

            assert(actualResult.score === expectedScore,
                `Actual code score equals to expected, expected: ${expectedScore} actual: ${actualResult.score}`);

        })
    });

    describe("Pascal sum with score 100 (input file/output file)", function () {
        this.slow(secondsToMs(60));
        it("should have analysis result, execution result and mark", async function () {
            //language=Pascal
            let codeToRun = `
                var
                    f1,f2:text;
                a,s: integer;

                begin
                assign(f1, 'input.txt');
                assign(f2, 'output.txt');
                reset(f1);

                s := 0;


                while not eof(f1) do
                    begin
                    read(f1,a);

                s := s+a;
                end;
                close(f1);

                rewrite(f2);
                write(f2,s);
                close(f2);
                end.
            `;

            let expectedScore = 100;

            let actualResult = await Api.fetchRequestForSumTask(languages.Pascal, codeToRun);

            assert.isNotNull(actualResult.codeAnalysisResult, "Analysis result is not null");
            assert.isNotNull(actualResult.codeExecutionResult, "Execution result is not null");

            assert.isTrue(actualResult.codeAnalysisResult.isSuccessful, "Analysis result successful");
            assert.isTrue(actualResult.codeExecutionResult.wasSuccessful, "Execution result successful");

            assert(actualResult.score === expectedScore,
                `Actual code score equals to expected, expected: ${expectedScore} actual: ${actualResult.score}`);

        })
    });

    describe("Pascal sum with score 100 (stdin/stdout)", function () {
        this.slow(secondsToMs(60));
        it("should have analysis result, execution result and mark", async function () {
            // language=Pascal
            let codeToRun = `
                var e,s:string; 
                    sum,k,i:integer;
                begin
                 readln(s);
                 i:=1;
                 while (i<=length(s)) do 
                 begin
                  e:='';
                  
                  while s[i] in ['0'..'9'] do 
                    begin 
                      e:=e+s[i];
                      inc(i);
                      
                      if(i > length(s))
                      then
                        break;
                    end;
                  k:=0;
                  if e<>'' then val(e,k);
                  sum:=sum+k;
                  inc(i);
                 end;
                 writeln(sum);
                
                end.`;

            let expectedScore = 100;

            let actualResult = await Api.fetchRequestForSumTask(languages.Pascal, codeToRun);

            assert.isNotNull(actualResult.codeAnalysisResult, "Analysis result is not null");
            assert.isNotNull(actualResult.codeExecutionResult, "Execution result is not null");

            assert.isTrue(actualResult.codeAnalysisResult.isSuccessful, "Analysis result successful");
            assert.isTrue(actualResult.codeExecutionResult.wasSuccessful, "Execution result successful");

            assert(actualResult.score === expectedScore,
                `Actual code score equals to expected, expected: ${expectedScore} actual: ${actualResult.score}`);

        })
    });

});
