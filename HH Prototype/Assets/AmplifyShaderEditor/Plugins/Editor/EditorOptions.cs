using UnityEngine;
using UnityEditor;

namespace AmplifyShaderEditor
{
	[System.Serializable]
	public class OptionsWindow
	{
		[SerializeField]
		public bool ColoredPorts = false;

		//[SerializeField]
		//public bool ExpandedMain = true;

		//[SerializeField]
		//public bool ExpandedBlend = false;

		//[SerializeField]
		//public bool ExpandedStencil = false;

		//[SerializeField]
		//public bool ExpandedTesselation = false;

		//[SerializeField]
		//public bool ExpandedDepth = false;

		//[SerializeField]
		//public bool ExpandedRendering = false;

		//[SerializeField]
		//public bool ExpandedProperties = false;

		public OptionsWindow()
		{
			//Load ();
		}

		public void Init()
		{
			Load();
		}

		public void Destroy()
		{
			Save();
		}

		public void Save()
		{
			EditorPrefs.SetBool( "ColoredPorts", UIUtils.CurrentWindow.ToggleDebug );
			EditorPrefs.SetBool( "ExpandedMain", UIUtils.CurrentWindow.ExpandedMain );
			EditorPrefs.SetBool( "ExpandedBlend", UIUtils.CurrentWindow.ExpandedBlend );
			EditorPrefs.SetBool( "ExpandedStencil", UIUtils.CurrentWindow.ExpandedStencil );
			EditorPrefs.SetBool( "ExpandedTesselation", UIUtils.CurrentWindow.ExpandedTesselation );
			EditorPrefs.SetBool( "ExpandedDepth", UIUtils.CurrentWindow.ExpandedDepth );
			EditorPrefs.SetBool( "ExpandedRendering", UIUtils.CurrentWindow.ExpandedRendering );
			EditorPrefs.SetBool( "ExpandedProperties", UIUtils.CurrentWindow.ExpandedProperties );
		}

		//public void Save(string name, bool value)
		//{
		//	EditorPrefs.SetBool( name, value );
		//}

		public void Load()
		{
			UIUtils.CurrentWindow.ToggleDebug = EditorPrefs.GetBool( "ColoredPorts" );
			ColoredPorts = UIUtils.CurrentWindow.ToggleDebug;
			UIUtils.CurrentWindow.ExpandedMain = EditorPrefs.GetBool( "ExpandedMain", true );
			UIUtils.CurrentWindow.ExpandedBlend = EditorPrefs.GetBool( "ExpandedBlend" );
			UIUtils.CurrentWindow.ExpandedStencil = EditorPrefs.GetBool( "ExpandedStencil" );
			UIUtils.CurrentWindow.ExpandedTesselation = EditorPrefs.GetBool( "ExpandedTesselation" );
			UIUtils.CurrentWindow.ExpandedDepth = EditorPrefs.GetBool( "ExpandedDepth" );
			UIUtils.CurrentWindow.ExpandedRendering = EditorPrefs.GetBool( "ExpandedRendering" );
			UIUtils.CurrentWindow.ExpandedProperties = EditorPrefs.GetBool( "ExpandedProperties" );
		}
	}
}
