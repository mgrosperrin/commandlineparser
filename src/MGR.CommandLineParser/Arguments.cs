using System;
using System.Collections.Generic;
using System.IO;

namespace MGR.CommandLineParser
{
    internal class Arguments
    {
        private readonly List<string> _arguments;
        private int _currentIndex = -1;
        public Arguments(IEnumerable<string> args)
        {
            _arguments = new List<string>(args);
        }

        public void Revert()
        {
            if (_currentIndex == 0)
            {
                throw new InvalidOperationException("Unable to revert Arguments: it is already at the start.");
            }
        }

        public bool Advance()
        {
            if (_arguments.Count == _currentIndex + 1)
            {
                return false;
            }
            _currentIndex++;
            ReplaceRspFileInCurrentPosition();
            return true;
        }

        private void ReplaceRspFileInCurrentPosition()
        {
            var current = GetCurrent();
            if (current.StartsWith("@", StringComparison.CurrentCulture))
            {
                if (!current.StartsWith("@@", StringComparison.CurrentCulture))
                {
                    var responseFileName = current.Remove(0, 1);
                    if (Path.GetExtension(responseFileName) == ".rsp" && File.Exists(responseFileName))
                    {
                        var responseFileContent = File.ReadAllLines(responseFileName);
                        _arguments.RemoveAt(_currentIndex);
                        _arguments.InsertRange(_currentIndex, responseFileContent);
                        ReplaceRspFileInCurrentPosition();
                        return;
                    }
                }
                var currentWithoutAt = current.Remove(0, 1);
                _arguments[_currentIndex] = currentWithoutAt;
            }
        }


        public string GetCurrent()
        {
            if (_currentIndex < 0 || _currentIndex >= _arguments.Count)
            {
                throw new ArgumentOutOfRangeException();
            }
            return _arguments[_currentIndex];
        }
    }
}
