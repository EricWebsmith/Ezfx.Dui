# C# win-form for python development 

In a data-related project, like machine learning, debugging the backend code can be torment. When you step into the code, you get the 2d-, 3d-or even nd-array. And they are not readable. This happens to other projects, too. For example, an html post process for a spider.



In machine learn, there is a concept called **data visualization**. I would argue that other projects may need visualization also. (like html processing) . Can we ease the pan of development by visualization? The answer is yes.



My solution is to develop a tool for the developers to use, which will visualize the steps. I call it **Developer User Interface (DUI)**. 



## Developer User Interface

The DUI is similar to the UI but is tailored for developers. The development team develop tools for the end users with UI, but they themselves need a tool in their daily work, too.



## Use C# Windows forms for GUI

Windows is still the most popular OS for individual developers. C# WinForms is the most popular GUI for windows. I compared some python GUI libraries like PyQT. I finally gave up the idea of any python GUI because they are not WYSIWYG (What You See is What You Get).



C# is similar to java. If you do not use any framework like sprint boot, they are almost identical.



## Call Python from C#

There are two ways to call python from C#. In the python 2 era, I use to use IronPython. But unfortunately, at this moment, IronPython 3, which is for python 3, is not available yet.



Thus, the only option left is to use the *Process* class in C#. Let me introduce this idea by three examples.

### Example 1: Hello World

Let me demonstrate the idea by a simple hello-world project.

Firstly, let us create hello_world.py with the following code.

```python
print("hello world!")
```

Now, we drag and drop two buttons and two labels.

![Hello World](images/hello_world.png)



We rename the controls to backgroundButton, foregroundButton, backgrounLabel, foregroundLabel respectively. When we double click backgroundButton, an event is created for us. like this:

```c#
        private void backgroundButton_Click(object sender, EventArgs e)
        {
        }
```

We can call the python code using Process class like that:

```c#
        private void backgroundButton_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = "python.exe";
            p.StartInfo.Arguments = "hello_world.py";
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();
            string result = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            backgroundLabel.Text = result;
        }
```

The above is what we do normally for the end user --  run backend code background. But here our end user is a developer. He/She wants to see the process. It will be very useful is something is wrong.



I firstly added an input statement at the end of the python file. But if there is an exception, the python script will just stop and it is hard to debug. After many experiments, I finally decide to create a BAT file and call the file.

```c#
        private void foregroundButton_Click(object sender, EventArgs e)
        {
            string bat_file = "run.bat";
            string bat_content = "echo start\n";
            bat_content += "python hello_world.py\n";
            // pause the batch script.
            // this is important when running in foreground.
            bat_content += "pause";
            File.WriteAllText(bat_file, bat_content);

            Process p = new Process();
            p.StartInfo.FileName = bat_file;
            p.Start();
        }
```

This is why I call it *DUI* instead of UI.



### Example 2: Linear Regression Plots

Suppose we are developing a machine learning package using Python. 

There is only one file in the package. It is linear_regression.py

```python
import numpy as np

class LinearRegression():
    def __init__(self, alpha=0.1, a=0, b=0, n_iterations=100):
        self.alpha = alpha
        self.a = a
        self.b = b
        self.n_iterations = 5
        self.n = 0
        self.x = []
        self.y = []

    def model(self):
        return self.a*self.x + self.b

    def cost(self):
        return 0.5/self.n * (np.square(self.y-self.a*self.x-self.b)).sum()

    def optimize(self):
        y_hat = self.model()
        da = (1.0/self.n) * ((y_hat-self.y)*self.x).sum()
        db = (1.0/self.n) * ((y_hat-self.y).sum())
        self.a = self.a - self.alpha*da
        self.b = self.b - self.alpha*db

    def fit(self, x, y):
        self.x = x
        self.y = y
        self.n = len(x)

        for i in range(self.n_iterations):
            self.optimize()
```



We have some developers to test it.

In our DUI, we firstly create a python script called visualization.py

```python
import numpy as np
import sys
import matplotlib.pyplot as plt
# start import your python project
sys.path.append(r"D:\projects\PySharp\2.PyCore+PyMedia+C#\PyCore")
from linear_regression import LinearRegression

def main():
    x = [13854,12213,11009,10655,9503]
    x = np.reshape(x,newshape=(5,1)) / 10000.0
    y =  [21332, 20162, 19138, 18621, 18016]
    y = np.reshape(y,newshape=(5,1)) / 10000.0

    params = []
    model = LinearRegression(alpha=0.2, n_iterations=5)
    model.x = x
    model.y = y
    model.n = 5

    for i in range(10):
        model.optimize()
        y_hat = model.model()
        params.append((model.a, model.b))
        plt.scatter(x,y)
        plt.plot(x, y_hat)
        plt.savefig(f"{i+1}.png")
        plt.cla()

    for a, b in params:
        print(a, b)

if __name__ == "__main__":
    main()
```

In order to call your python package under development, you need to append the path using the following statement:

```python
sys.path.append(r"D:\projects\PySharp\2.PyCore+PyMedia+C#\PyCore")
```

The makes your python code work but it cannot be debugged. You have to tell the IDE how to debug into the package. 

### Debug into the Package

Besides, you also need to set up the IDE. I use VSCode. In the settings.json file under the .vscode folder, you notify the IDE to find the environment file.

```json
{
    "python.pythonPath": "C:\\ProgramData\\Anaconda3\\python.exe",
    "python.envFile": "${workspaceFolder}/.env"
}
```

In your working folder, you add a file named .env and add the following.

```bash
PYTHONPATH = "D:\projects\PySharp\2.PyCore+PyMedia+C#\PyCore"
```

Drag linear_regression.py into VSCode. Set a breakpoint. Run visualize.py. It will stop in linear_regression.py.

### Use C# Picture Control

There many controls in C# winforms. You can put the pictures we just created in a picture control. 

```c#
this.linePictureBox.Image = Image.FromFile(path);
```

## Example 3: Use HTML Pages

If you are familiar with HTML, you can create a HTML page using python. In C# winforms, you just drag and drop a WebBrowser control and add the following code to the button click event. 

```c#
webBrowser1.Navigate(Directory.GetCurrentDirectory()+"/py/display.html");
```

In python, matplotlib is used to create charts. If you want to create other kinds of pictures, like drawing a picture with different lines and shapes ,you can use Open CV. 

## Conclusion

Developer User Interface is an uncharted area. It has been neglected so long. If we do not even know how to automate our own work, how can we automate others' work? I hope this article helps you and makes your development easier.











