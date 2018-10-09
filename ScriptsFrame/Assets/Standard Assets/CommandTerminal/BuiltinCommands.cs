using System.Text;
using System.Diagnostics;
using UnityEngine;
using JacobGames.SuperInvoke;

namespace CommandTerminal
{
    public static class BuiltinCommands
    {
        [RegisterCommand(Help = "Does nothing")]
        static void CommandNoop(CommandArg[] args) { }

        [RegisterCommand(Help = "Clears the Command Console", MaxArgCount = 0)]
        static void CommandClear(CommandArg[] args) {
            Terminal.Buffer.Clear();
        }

        [RegisterCommand(Help = "Lists all Commands or displays help documentation of a Command", MaxArgCount = 1)]
        static void CommandHelp(CommandArg[] args) {
            if (args.Length == 0) {
                foreach (var command in Terminal.Shell.Commands) {
                    Terminal.Log("{0}: {1}", command.Key.PadRight(16), command.Value.help);
                }
                return;
            }

            string command_name = args[0].String.ToUpper();

            if (!Terminal.Shell.Commands.ContainsKey(command_name)) {
                Terminal.Shell.IssueErrorMessage("Command {0} could not be found.", command_name);
                return;
            }

            string help = Terminal.Shell.Commands[command_name].help;

            if (help == null) {
                Terminal.Log("{0} does not provide any help documentation.", command_name);
            } else {
                Terminal.Log(help);
            }
        }

        [RegisterCommand(Help = "Times the execution of a Command", MinArgCount = 1)]
        static void CommandTime(CommandArg[] args) {
            var sw = new Stopwatch();
            sw.Start();

            Terminal.Shell.RunCommand(JoinArguments(args));

            sw.Stop();
            Terminal.Log("Time: {0}ms", (double)sw.ElapsedTicks / 10000);
        }

        [RegisterCommand(Help = "Outputs message")]
        static void CommandPrint(CommandArg[] args) {
            Terminal.Log(JoinArguments(args));
        }

    #if DEBUG
        [RegisterCommand(Help = "Outputs the StackTrace of the previous message", MaxArgCount = 0)]
        static void CommandTrace(CommandArg[] args) {
            int log_count = Terminal.Buffer.Logs.Count;

            if (log_count - 2 <  0) {
                Terminal.Log("Nothing to trace.");
                return;
            }

            var log_item = Terminal.Buffer.Logs[log_count - 2];

            if (log_item.stack_trace == "") {
                Terminal.Log("{0} (no trace)", log_item.message);
            } else {
                Terminal.Log(log_item.stack_trace);
            }
        }
    #endif

        [RegisterCommand(Help = "Quits running Application", MaxArgCount = 0)]
        static void CommandQuit(CommandArg[] args) {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        }

        static string JoinArguments(CommandArg[] args) {
            var sb = new StringBuilder();
            int arg_length = args.Length;

            for (int i = 0; i < arg_length; i++) {
                sb.Append(args[i].String);

                if (i < arg_length - 1) {
                    sb.Append(" ");
                }
            }

            return sb.ToString();
        }

        [RegisterCommand(Help = "Open Osk KeyBoard")]
        static void CommandOpenOSK(CommandArg[] args)
        {
            //这边操作必须要先把鼠标移开才能操作，不然会直接输入
            KeyBoardEvent.DoMouseClick(Screen.width/2, Screen.height/2);
            //SuperInvoke.SkipFrames 等待帧数
            SuperInvoke.SkipFrames(1, () => {KeyBoardEvent.ClickFNum(118);
                //鼠标恢复原位
                KeyBoardEvent.DoMouseClick(KeyBoardEvent.lastPoint.X, KeyBoardEvent.lastPoint.Y);
            });
        }

        //[RegisterCommand(Help = "Drag The Window")]
        //static void CommandDrag(CommandArg[] args)
        //{
        //    KeyBoardEvent.DoMouseClick(Screen.width / 2, Screen.height / 2);
        //    SuperInvoke.SkipFrames(1, () => { KeyBoardEvent.ClickFNum(119);
        //        KeyBoardEvent.DoMouseClick(KeyBoardEvent.lastPoint.X, KeyBoardEvent.lastPoint.Y);
        //    });
        //}

        [RegisterCommand(Help = "Show RightDown Debug")]
        static void CommandDebug(CommandArg[] args)
        {
            KeyBoardEvent.DoMouseClick(Screen.width / 2, Screen.height / 2);
            SuperInvoke.SkipFrames(1, () => { KeyBoardEvent.ClickFNum(120);
                KeyBoardEvent.DoMouseClick(KeyBoardEvent.lastPoint.X, KeyBoardEvent.lastPoint.Y);
            });
        }

        [RegisterCommand(Help = "Show FPS")]
        static void CommandFPS(CommandArg[] args)
        {
            KeyBoardEvent.DoMouseClick(Screen.width / 2, Screen.height / 2);
            SuperInvoke.SkipFrames(1, () => { KeyBoardEvent.ClickFNum(121);
                KeyBoardEvent.DoMouseClick(KeyBoardEvent.lastPoint.X, KeyBoardEvent.lastPoint.Y);
            });
        }

        [RegisterCommand(Help = "TimeScale = 0")]
        static void CommandPause(CommandArg[] args)
        {
            KeyBoardEvent.DoMouseClick(Screen.width / 2, Screen.height / 2);
            SuperInvoke.SkipFrames(1, () => { KeyBoardEvent.ClickFNum(122);
                KeyBoardEvent.DoMouseClick(KeyBoardEvent.lastPoint.X, KeyBoardEvent.lastPoint.Y);
            });
        }

        [RegisterCommand(Help = "TimeScale = 1 or 50")]
        static void CommandSpeed(CommandArg[] args)
        {
            KeyBoardEvent.DoMouseClick(Screen.width / 2, Screen.height / 2);
            SuperInvoke.SkipFrames(1, () => { KeyBoardEvent.ClickFNum(123);
                KeyBoardEvent.DoMouseClick(KeyBoardEvent.lastPoint.X, KeyBoardEvent.lastPoint.Y);
            });
        }
    }
}
