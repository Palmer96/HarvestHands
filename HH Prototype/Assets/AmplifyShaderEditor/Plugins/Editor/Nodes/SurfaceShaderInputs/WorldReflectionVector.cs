// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using System;
using UnityEngine;
namespace AmplifyShaderEditor
{
	[Serializable]
	[NodeAttributes( "World Reflection", "Surface Standard Inputs", "Per pixel world reflection vector", null, KeyCode.R )]
	public sealed class WorldReflectionVector : ParentNode
	{
		protected override void CommonInit( int uniqueId )
		{
			base.CommonInit( uniqueId );
			AddInputPort( WirePortDataType.FLOAT3, false, "Normal" );
			AddOutputVectorPorts( WirePortDataType.FLOAT3, "XYZ" );

			UIUtils.AddNormalDependentCount();
		}

		public override void Destroy()
		{
			base.Destroy();
			UIUtils.RemoveNormalDependentCount();
		}

		public override void PropagateNodeData( NodeData nodeData )
		{
			base.PropagateNodeData( nodeData );
			if ( m_inputPorts[ 0 ].IsConnected )
				UIUtils.CurrentDataCollector.DirtyNormal = true;
		}

		public override string GenerateShaderForOutput( int outputId, ref MasterNodeDataCollector dataCollector, bool ignoreLocalVar )
		{

			dataCollector.AddToInput( m_uniqueId, UIUtils.GetInputDeclarationFromType( m_currentPrecisionType, AvailableSurfaceInputs.WORLD_REFL ), true );
			dataCollector.AddToInput( m_uniqueId, Constants.InternalData, false );
			string result = string.Empty;
			if ( m_inputPorts[ 0 ].IsConnected )
			{
				result = "WorldReflectionVector( " + Constants.InputVarStr + " , " + m_inputPorts[ 0 ].GenerateShaderForOutput( ref dataCollector, WirePortDataType.FLOAT3, ignoreLocalVar ) + " )";
				//if ( !dataCollector.DirtyNormal )
				//{
					dataCollector.ForceNormal = true;
				//}
			} else
			{
				if ( !dataCollector.DirtyNormal )
					result = Constants.InputVarStr + ".worldRefl";
				else
					result = "WorldReflectionVector( " + Constants.InputVarStr + " , float3(0,0,1) )";
			}


			return GetOutputVectorItem( 0, outputId, result );
		}

	}
}
