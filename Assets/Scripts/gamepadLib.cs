using UnityEngine;
using System.Collections;

namespace GamePadInput {
	public struct AxisCode {
		#region Public Fields
		/// <summary>
		/// This X-axis axis code.
		/// </summary>
		public string x;
		/// <summary>
		/// The Y-axis axis code.
		/// </summary>
		public string y;
		#endregion Public Fields

		#region Constructors
		public AxisCode (string x, string y) {
			this.x = x;
			this.y = y;
		}
		#endregion Constructors
	}

	/// <summary>
	/// Gamepad	
	/// </summary>
	public static class GamePad {
		#region Enums
		private enum Button {A, B, X, Y, LeftShoulder, RightShoulder, Select, Start, LeftStickIn, RightStickIn}
		private enum Axis {LeftStick, RightStick, Dpad}
		private enum Trigger {LeftTrigger, RightTrigger}
		#endregion Enums

		public static GamePadKeyCodes gamepadKeyCodes = new GamePadKeyCodes(0);

		///<summary>
		/// Struct holding all gamepad input keycodes.
		///</summary>
		public struct GamePadKeyCodes {
			public KeyCode Btn_A;
			public KeyCode Btn_B;
			public KeyCode Btn_X;
			public KeyCode Btn_Y;
			public KeyCode Btn_ShoulderLeft;
			public KeyCode Btn_ShoulderRight;
			public KeyCode Btn_Select;
			public KeyCode Btn_Start;
			public KeyCode Btn_StickLeftClick;
			public KeyCode Btn_StickRightClick;
			public AxisCode Axis_Dpad;
			public AxisCode Axis_StickLeft;
			public AxisCode Axis_StickRight;

			public GamePadKeyCodes(int i) {
				Btn_A = GetKeyCode (Button.A);
				Btn_B = GetKeyCode (Button.B);
				Btn_X = GetKeyCode (Button.X);
				Btn_Y = GetKeyCode (Button.Y);
				Btn_ShoulderLeft    = GetKeyCode (Button.LeftShoulder);
				Btn_ShoulderRight   = GetKeyCode (Button.RightShoulder);
				Btn_Select          = GetKeyCode (Button.Select);
				Btn_Start           = GetKeyCode (Button.Start);
				Btn_StickLeftClick  = GetKeyCode (Button.LeftStickIn);
				Btn_StickRightClick = GetKeyCode (Button.RightStickIn);
				Axis_Dpad 		= GetAxisCode (Axis.Dpad);
				Axis_StickLeft 	= GetAxisCode (Axis.LeftStick);
				Axis_StickRight = GetAxisCode (Axis.RightStick);
			}
		}

		#region Key Finds
		/// <summary>
		/// Gets the KeyCode of the submitted gamepad face button.
		/// </summary>
		private static KeyCode GetKeyCode(Button btn) {
			if (btn == Button.A) {
				return (KeyCode.JoystickButton0);
			} else if (btn == Button.B) {
				return (KeyCode.JoystickButton1);
			} else if (btn == Button.X) {
				return (KeyCode.JoystickButton2);
			} else if (btn == Button.Y) {
				return (KeyCode.JoystickButton3);
			} else if (btn == Button.LeftShoulder) {
				return (KeyCode.JoystickButton4);
			} else if (btn == Button.RightShoulder) {
				return (KeyCode.JoystickButton5);
			} else if (btn == Button.Select) {
				return (KeyCode.JoystickButton6);
			} else if (btn == Button.Start) {
				return (KeyCode.JoystickButton7);
			} else if (btn == Button.LeftStickIn) {
				return (KeyCode.JoystickButton8);
			} else if (btn == Button.RightStickIn) {
				return (KeyCode.JoystickButton9);
			} else {
				return (KeyCode.None);
			}
		}

		private static AxisCode GetAxisCode (Axis axis) {
			AxisCode axisData;

			if (axis == Axis.Dpad) {
				axisData.x = "DPad_XAxis_0";
				axisData.y = "DPad_YAxis_0";
			} else if (axis == Axis.LeftStick) {
				axisData.x = "L_XAxis_0";
				axisData.y = "L_YAxis_0";
			} else if (axis == Axis.RightStick) {
				axisData.x = "R_XAxis_0";
				axisData.y = "R_YAxis_0";
			} else {
				axisData.x = "";
				axisData.y = "";
			}

			return (axisData);
		}
		#endregion Key Finds
	}
}