// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
//
// Custom Node Vertex Binormal World
// Donated by Community Member Kebrus

using System;

namespace AmplifyShaderEditor
{
	[Serializable]
	[NodeAttributes( "World Bitangent", "Surface Standard Inputs", "Per pixel world bitangent vector" )]
	public sealed class VertexBinormalNode : ParentNode
	{
		protected override void CommonInit( int uniqueId )
		{
			base.CommonInit( uniqueId );
			AddOutputVectorPorts( WirePortDataType.FLOAT3, "XYZ" );
		}

		public override void PropagateNodeData( NodeData nodeData )
		{
			base.PropagateNodeData( nodeData );
			UIUtils.CurrentDataCollector.DirtyNormal = true;
		}

		public override string GenerateShaderForOutput( int outputId, ref MasterNodeDataCollector dataCollector, bool ignoreLocalvar )
		{
			//if ( !dataCollector.DirtyNormal )
			//{
				dataCollector.ForceNormal = true;
			//}

			dataCollector.AddToInput( m_uniqueId, UIUtils.GetInputDeclarationFromType( m_currentPrecisionType, AvailableSurfaceInputs.WORLD_NORMAL ), true );
			dataCollector.AddToInput( m_uniqueId, Constants.InternalData, false );

			dataCollector.AddToLocalVariables( m_uniqueId, m_currentPrecisionType, WirePortDataType.FLOAT3, "worldBitangent", "WorldNormalVector( " + Constants.InputVarStr + ", float3(0,1,0) )" );
			
			//dataCollector.AddToInput( m_uniqueId, "float3 worldBinormal", true );
			//dataCollector.AddVertexInstruction( Constants.VertexShaderOutputStr + ".worldBinormal = normalize( cross( UnityObjectToWorldNormal(" + Constants.VertexShaderInputStr+ ".normal), UnityObjectToWorldDir(" + Constants.VertexShaderInputStr + ".tangent.xyz) ) * "+ Constants.VertexShaderInputStr+ ".tangent.w * unity_WorldTransformParams.w )", m_uniqueId );

			return "worldBitangent";
		}
	}
}

