using System;
using Python.Runtime;

namespace WebApplication1;

public class ModelWrapper : IDisposable
{
    private dynamic _pythonInstance;

    public ModelWrapper(string pathToCsv, string pathToPython)
    {
        PythonEngine.Initialize();
        using (Py.GIL())
        {
            dynamic sys = Py.Import("sys");
            sys.path.append(pathToPython);
            dynamic pyModule = Py.Import("MyModel");
            _pythonInstance = pyModule.MyModel(pathToCsv);
        }
    }

    public int[] Predict()
    {
        using (Py.GIL())
        {
            dynamic predictions = _pythonInstance.predict();
            return ((PyObject)predictions).As<int[]>();
        }
    }

    public void Dispose()
    {
        PythonEngine.Shutdown();
    }
}