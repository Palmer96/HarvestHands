// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
using System;
using UnityEngine;
using UnityEditor;

namespace AmplifyShaderEditor
{
	public enum ViewDirSpace
	{
		Tangent,
		World
	}

	[System.Serializable]
	[NodeAttributes( "View Dir", "Surface Standard Inputs", "View direction vector" )]
	public sealed class ViewDirInputsCoordNode : SurfaceShaderINParentNode
	{
		private const string SpaceStr = "Space";
		private const string WorldDirVarStr = "worldViewDir";
		//private readonly string WorldDirVarDecStr = "{0} {1};";
		//private readonly string WorldDirVarDefStr = string.Format( "{0}.{1} = normalize( _WorldSpaceCameraPos - {2}.vertex )", Constants.VertexShaderOutputStr, WorldDirVarStr , Constants.VertexShaderInputStr );
		//private readonly string WorldDirVarOnFrag = Constants.InputVarStr + "." + WorldDirVarStr;
		[SerializeField]
		private ViewDirSpace m_viewDirSpace = ViewDirSpace.World;

		[SerializeField]
		private bool m_addInstruction = false;

		public override void Reset()
		{
			base.Reset();
			m_addInstruction = true;
		}

		protected override void CommonInit( int uniqueId )
		{
			base.CommonInit( uniqueId );
			m_currentInput = AvailableSurfaceInputs.VIEW_DIR;
			InitialSetup();
			m_textLabelWidth = 60;
			UpdateTitle();
		}

		private void UpdateTitle()
		{
			m_additionalContent.text = string.Format( "( {0} )", m_viewDirSpace.ToString() );
			m_titleLineAdjust = 5;
			m_sizeIsDirty = true;
		}

		public override void DrawProperties()
		{
			base.DrawProperties();
			EditorGUI.BeginChangeCheck();
			m_viewDirSpace = ( ViewDirSpace ) EditorGUILayout.EnumPopup( SpaceStr, m_viewDirSpace );
			if ( EditorGUI.EndChangeCheck() )
			{
				UpdateTitle();
			}
		}

		public override void PropagateNodeData( NodeData nodeData )
		{
			base.PropagateNodeData( nodeData );
			if ( m_viewDirSpace == ViewDirSpace.Tangent )
				UIUtils.CurrentDataCollector.DirtyNormal = true;
		}

		public override string GenerateShaderForOutput( int outputId, ref MasterNodeDataCollector dataCollector, bool ignoreLocalVar )
		{
			if ( dataCollector.PortCategory == MasterNodePortCategory.Vertex || dataCollector.PortCategory == MasterNodePortCategory.Tessellation )
			{
				if ( m_addInstruction )
				{
					string precision = UIUtils.FinalPrecisionWirePortToCgType( m_currentPrecisionType, WirePortDataType.FLOAT3 );
					dataCollector.AddVertexInstruction( precision + " viewDir = normalize( _WorldSpaceCameraPos - "+ Constants.VertexShaderInputStr + ".vertex )", m_uniqueId );
					m_addInstruction = false;
				}

				return GetOutputVectorItem( 0, outputId, "viewDir" );
			}
			else
			{
				if ( m_viewDirSpace == ViewDirSpace.World )
				{
					if ( dataCollector.DirtyNormal )
					{
						dataCollector.AddToInput( m_uniqueId, UIUtils.GetInputDeclarationFromType( m_currentPrecisionType, AvailableSurfaceInputs.WORLD_POS ), true );
						dataCollector.AddToLocalVariables( m_uniqueId, m_currentPrecisionType, WirePortDataType.FLOAT3, WorldDirVarStr, "normalize( UnityWorldSpaceViewDir( " + Constants.InputVarStr + ".worldPos ) )" );
						return WorldDirVarStr;

						//dataCollector.AddToInput( m_uniqueId, string.Format( WorldDirVarDecStr, UIUtils.FinalPrecisionWirePortToCgType( m_currentPrecisionType, WirePortDataType.FLOAT3 ), WorldDirVarStr ), false );
						//dataCollector.AddVertexInstruction( WorldDirVarDefStr, m_uniqueId );
						//return WorldDirVarOnFrag;
					}
					else
					{
						return base.GenerateShaderForOutput( outputId, ref dataCollector, ignoreLocalVar );
					}
				}
				else
				{
					//if ( !dataCollector.DirtyNormal )
					//{
						dataCollector.ForceNormal = true;
					//}
					return base.GenerateShaderForOutput( outputId, ref dataCollector, ignoreLocalVar );
				}
			}
		}

		public override void ReadFromString( ref string[] nodeParams )
		{
			base.ReadFromString( ref nodeParams );
			if ( UIUtils.CurrentShaderVersion() > 2402 )
				m_viewDirSpace = ( ViewDirSpace )Enum.Parse( typeof( ViewDirSpace ), GetCurrentParam( ref nodeParams ) );

			UpdateTitle();
		}

		public override void WriteToString( ref string nodeInfo, ref string connectionsInfo )
		{
			base.WriteToString( ref nodeInfo, ref connectionsInfo );
			IOUtils.AddFieldValueToString( ref nodeInfo, m_viewDirSpace );
		}
	}
}
