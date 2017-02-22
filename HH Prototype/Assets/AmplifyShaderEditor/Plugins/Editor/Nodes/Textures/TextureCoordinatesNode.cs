// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace AmplifyShaderEditor
{
	[Serializable]
	[NodeAttributes( "Texture Coordinates", "Surface Standard Inputs", "Texture UV coordinates set", null, KeyCode.U )]
	public sealed class TextureCoordinatesNode : ParentNode
	{
		private readonly string[] Dummy = { string.Empty };

		private const string TilingStr = "Tiling";
		private const string OffsetStr = "Offset";
		private const string TexCoordStr = "texcoord_";

		[SerializeField]
		private int m_referenceArrayId = -1;

		[SerializeField]
		private int m_referenceNodeId = -1;

		[SerializeField]
		private int m_textureCoordChannel = 0;

		[SerializeField]
		private int m_texcoordId = -1;

		[SerializeField]
		private string m_surfaceTexcoordName = string.Empty;

		private bool m_forceNodeUpdate = false;
		private TexturePropertyNode m_referenceNode = null;

		protected override void CommonInit( int uniqueId )
		{
			base.CommonInit( uniqueId );
			AddInputPort( WirePortDataType.FLOAT2, false, "Tiling" );
			m_inputPorts[ 0 ].Vector2InternalData = new Vector2( 1, 1 );
			AddInputPort( WirePortDataType.FLOAT2, false, "Offset" );
			AddOutputVectorPorts( WirePortDataType.FLOAT2, "UV" );
			m_outputPorts[ 1 ].Name = "U";
			m_outputPorts[ 2 ].Name = "V";
			m_textLabelWidth = 75;
			m_useInternalPortData = true;
			m_inputPorts[ 0 ].Category = MasterNodePortCategory.Vertex;
			m_inputPorts[ 1 ].Category = MasterNodePortCategory.Vertex;
		}

		public override void Reset()
		{
			m_texcoordId = -1;
			m_surfaceTexcoordName = string.Empty;
		}

		void UpdateTitle()
		{
			if ( m_referenceArrayId > -1 && m_referenceNode != null )
			{
				m_referenceNode = UIUtils.GetTexturePropertyNode( m_referenceArrayId );
				m_additionalContent.text = string.Format( "Value( {0} )", m_referenceNode.PropertyInspectorName );
				m_titleLineAdjust = 5;
				m_sizeIsDirty = true;
			}
			else
			{
				m_additionalContent.text = string.Empty;
				m_titleLineAdjust = 0;
				m_sizeIsDirty = true;
			}
		}

		void UpdatePorts()
		{
			if ( m_referenceArrayId > -1 )
			{
				m_inputPorts[ 0 ].Locked = true;
				m_inputPorts[ 1 ].Locked = true;
			}
			else
			{
				m_inputPorts[ 0 ].Locked = false;
				m_inputPorts[ 1 ].Locked = false;
			}
		}

		public override void DrawProperties()
		{
			EditorGUI.BeginChangeCheck();
			List<string> arr = new List<string>( UIUtils.TexturePropertyNodeArr() );
			bool guiEnabledBuffer = GUI.enabled;
			if ( arr != null && arr.Count > 0 )
			{
				arr.Insert( 0, "None" );
				GUI.enabled = true;
				m_referenceArrayId = EditorGUILayout.Popup( Constants.AvailableReferenceStr, m_referenceArrayId + 1, arr.ToArray() ) - 1;
			}
			else
			{
				m_referenceArrayId = -1;
				GUI.enabled = false;
				m_referenceArrayId = EditorGUILayout.Popup( Constants.AvailableReferenceStr, m_referenceArrayId + 1, Dummy );
			}



			GUI.enabled = guiEnabledBuffer;
			if ( EditorGUI.EndChangeCheck() )
			{
				m_referenceNode = UIUtils.GetTexturePropertyNode( m_referenceArrayId );
				if ( m_referenceNode != null )
				{
					m_referenceNodeId = m_referenceNode.UniqueId;
				}
				else
				{
					m_referenceNodeId = -1;
					m_referenceArrayId = -1;
				}

				UpdateTitle();
				UpdatePorts();
			}

			m_textureCoordChannel = EditorGUILayout.IntPopup( Constants.AvailableUVSetsLabel, m_textureCoordChannel, Constants.AvailableUVSetsStr, Constants.AvailableUVSets );

			if ( m_referenceArrayId > -1 )
				GUI.enabled = false;

			base.DrawProperties();

			GUI.enabled = guiEnabledBuffer;
		}

		public override void Draw( DrawInfo drawInfo )
		{
			base.Draw( drawInfo );

			if ( m_forceNodeUpdate )
			{
				m_forceNodeUpdate = false;
				if ( UIUtils.CurrentShaderVersion() > 2404 )
				{
					m_referenceNode = UIUtils.GetNode( m_referenceNodeId ) as TexturePropertyNode;
					m_referenceArrayId = UIUtils.GetTexturePropertyNodeRegisterId( m_referenceNodeId );
				}
				else
				{
					m_referenceNode = UIUtils.GetTexturePropertyNode( m_referenceArrayId );
					if ( m_referenceNode != null )
					{
						m_referenceNodeId = m_referenceNode.UniqueId;
					}
				}
				UpdateTitle();
				UpdatePorts();
			}

			if ( m_referenceNode == null && m_referenceNodeId > -1 )
			{
				m_referenceNodeId = -1;
				m_referenceArrayId = -1;
				UpdateTitle();
				UpdatePorts();
			}
		}
		public override void ReadFromString( ref string[] nodeParams )
		{
			base.ReadFromString( ref nodeParams );
			m_textureCoordChannel = Convert.ToInt32( GetCurrentParam( ref nodeParams ) );
			if ( UIUtils.CurrentShaderVersion() > 2402 )
			{
				if ( UIUtils.CurrentShaderVersion() > 2404 )
				{
					m_referenceNodeId = Convert.ToInt32( GetCurrentParam( ref nodeParams ) );
				}
				else
				{
					m_referenceArrayId = Convert.ToInt32( GetCurrentParam( ref nodeParams ) );
				}

				m_forceNodeUpdate = true;
			}
		}

		public override void PropagateNodeData( NodeData nodeData )
		{
			UIUtils.SetCategoryInBitArray( ref m_category, nodeData.Category );

			MasterNodePortCategory propagateCategory = ( nodeData.Category!= MasterNodePortCategory.Vertex && nodeData.Category!= MasterNodePortCategory.Tessellation ) ? MasterNodePortCategory.Vertex : nodeData.Category;
			nodeData.Category = propagateCategory;
			int count = m_inputPorts.Count;
			for ( int i = 0; i < count; i++ )
			{
				if ( m_inputPorts[ i ].IsConnected )
				{
					//m_inputPorts[ i ].GetOutputNode().PropagateNodeCategory( category );
					m_inputPorts[ i ].GetOutputNode().PropagateNodeData( nodeData );
				}
			}
		}

		public override void WriteToString( ref string nodeInfo, ref string connectionsInfo )
		{
			base.WriteToString( ref nodeInfo, ref connectionsInfo );
			IOUtils.AddFieldValueToString( ref nodeInfo, m_textureCoordChannel );
			IOUtils.AddFieldValueToString( ref nodeInfo, ( ( m_referenceNode != null ) ? m_referenceNode.UniqueId : -1 ) );
		}

		public override string GenerateShaderForOutput( int outputId, ref MasterNodeDataCollector dataCollector, bool ignoreLocalVar )
		{
			if ( dataCollector.PortCategory == MasterNodePortCategory.Tessellation )
			{
				UIUtils.ShowMessage( m_nodeAttribs.Name + " cannot be used on Master Node Tessellation port" );
				return "-1";
			}

			bool isVertex = ( dataCollector.PortCategory == MasterNodePortCategory.Vertex || dataCollector.PortCategory == MasterNodePortCategory.Tessellation );

			if ( m_referenceArrayId > -1 )
			{
				//TexturePropertyNode node = UIUtils.GetTexturePropertyNode( m_referenceArrayId );
				m_referenceNode = UIUtils.GetTexturePropertyNode( m_referenceArrayId );
				if ( m_referenceNode != null )
				{
					string propertyName = m_referenceNode.PropertyName;
					int coordSet = ( ( m_textureCoordChannel < 0 ) ? 0 : m_textureCoordChannel );
					string uvName = string.Empty;

					string dummyPropUV = "_texcoord" + ( coordSet > 0 ? ( coordSet + 1 ).ToString() : "" );
					string dummyUV = "uv" + ( coordSet > 0 ? ( coordSet + 1 ).ToString() : "" ) + dummyPropUV;

					if ( isVertex )
					{
						//uvName = /*Constants.VertexShaderInputStr + "." + */IOUtils.GetVertexUVChannelName( coordSet );
						uvName = IOUtils.GetUVChannelName( propertyName, coordSet );
						string vertexInput = Constants.VertexShaderInputStr + ".texcoord";
						if ( coordSet > 0 )
						{
							vertexInput += coordSet.ToString();
						}

						dataCollector.AddToVertexLocalVariables( m_uniqueId, "float2 "+ uvName + " = "+ vertexInput + " * " + propertyName + "_ST.xy + " + propertyName + "_ST.zw;" );
					}
					else
					{
						//uvName = Constants.VertexShaderInputStr + ".texcoord"; ///*( isVertex ? Constants.VertexShaderOutputStr : Constants.InputVarStr ) + "." + */IOUtils.GetUVChannelName( propertyName, coordSet );
						uvName = IOUtils.GetUVChannelName( propertyName, coordSet );

						dataCollector.AddToLocalVariables( m_uniqueId, PrecisionType.Float, WirePortDataType.FLOAT2, IOUtils.GetUVChannelName( propertyName, coordSet ), Constants.InputVarStr + "." + dummyUV + " * " + propertyName + "_ST.xy + " + propertyName + "_ST.zw" );
					}

					//string uvChannelDeclaration = IOUtils.GetUVChannelDeclaration( propertyName, -1, coordSet );
					//dataCollector.AddToInput( m_uniqueId, uvChannelDeclaration, true );

					//string dummyPropUV = "_texcoord" + ( coordSet > 0 ? ( coordSet + 1 ).ToString() : "" );
					//string dummyUV = "uv" + ( coordSet > 0 ? ( coordSet + 1 ).ToString() : "" ) + dummyPropUV;

					dataCollector.AddToUniforms( m_uniqueId, "uniform float4 " + propertyName + "_ST;" );
					dataCollector.AddToProperties( m_uniqueId, "[HideInInspector] " + dummyPropUV + "( \"\", 2D ) = \"white\" {}", 100 );
					dataCollector.AddToInput( m_uniqueId, "float2 " + dummyUV, true );
					//dataCollector.AddToLocalVariables( m_uniqueId, PrecisionType.Float, WirePortDataType.FLOAT2, IOUtils.GetUVChannelName( propertyName, coordSet ), Constants.InputVarStr + "." + dummyUV + " * " + propertyName + "_ST.xy + " + propertyName + "_ST.zw" );


					return GetOutputVectorItem( 0, outputId, uvName );

					//dataCollector.AddToInput( m_uniqueId, IOUtils.GetUVChannelDeclaration( node.PropertyName, -1, -1 ), true );
					//return Constants.InputVarStr+"."+ IOUtils.GetUVChannelName( node.PropertyName, -1 );
				}
			}

			if ( m_texcoordId < 0 )
			{

				m_texcoordId = dataCollector.AvailableUvIndex;
				string texcoordName = TexCoordStr + m_texcoordId;

				bool tessVertexMode = isVertex && dataCollector.TesselationActive;

				string uvChannel = m_textureCoordChannel == 0 ? ".xy" : m_textureCoordChannel + ".xy";

				MasterNodePortCategory portCategory = dataCollector.PortCategory;
				if ( dataCollector.PortCategory != MasterNodePortCategory.Vertex && dataCollector.PortCategory != MasterNodePortCategory.Tessellation )
					dataCollector.PortCategory = MasterNodePortCategory.Vertex;

				// We need to reset local variables if there are already created to force them to be created in the vertex function
				int buffer = m_texcoordId;
				UIUtils.CurrentWindow.CurrentGraph.ResetNodesLocalVariables( this );
				
				bool dirtySpecialVarsBefore = dataCollector.DirtySpecialLocalVariables;
				bool dirtyVertexVarsBefore = dataCollector.DirtyVertexVariables;

				string tiling = m_inputPorts[ 0 ].GenerateShaderForOutput( ref dataCollector, WirePortDataType.FLOAT2, false, true );
				string offset = m_inputPorts[ 1 ].GenerateShaderForOutput( ref dataCollector, WirePortDataType.FLOAT2, false, true );
				dataCollector.PortCategory = portCategory;

				string vertexUV = Constants.VertexShaderInputStr + ".texcoord" + uvChannel;

				if ( !tessVertexMode )
					dataCollector.AddToInput( m_uniqueId, "float2 " + texcoordName, true );

				bool resetLocals = false;
				// new texture coordinates are calculated on the vertex shader so we need to register its local vars
				if ( !dirtySpecialVarsBefore && dataCollector.DirtySpecialLocalVariables )
				{
					dataCollector.AddVertexInstruction( UIUtils.CurrentDataCollector.SpecialLocalVariables, m_uniqueId, false );
					UIUtils.CurrentDataCollector.ClearSpecialLocalVariables();
					resetLocals = true;
				}

				if ( !dirtyVertexVarsBefore && dataCollector.DirtyVertexVariables )
				{
					dataCollector.AddVertexInstruction( UIUtils.CurrentDataCollector.VertexLocalVariables, m_uniqueId, false );
					UIUtils.CurrentDataCollector.ClearVertexLocalVariables();
					resetLocals = true;
				}

				//Reset local variables again so they wont be caught on the fragment shader
				if ( resetLocals )
					UIUtils.CurrentWindow.CurrentGraph.ResetNodesLocalVariables( this );


				if ( tessVertexMode )
				{
					dataCollector.AddVertexInstruction( vertexUV + " = " + vertexUV + " * " + tiling + " + " + offset, m_uniqueId );
					m_surfaceTexcoordName = Constants.VertexShaderInputStr + "." + IOUtils.GetVertexUVChannelName( m_textureCoordChannel ) + ".xy";
				}
				else
				{
					if ( dataCollector.TesselationActive )
					{
						dataCollector.AddVertexInstruction( vertexUV + " = " + vertexUV + " * " + tiling + " + " + offset, m_uniqueId );
					}
					else
					{
						dataCollector.AddVertexInstruction( Constants.VertexShaderOutputStr + "." + texcoordName + ".xy = " + vertexUV + " * " + tiling + " + " + offset, m_uniqueId );
					}

					m_surfaceTexcoordName = ( isVertex ? Constants.VertexShaderOutputStr : Constants.InputVarStr ) + "." + texcoordName;
				}

				m_texcoordId = buffer;
			}

			return GetOutputVectorItem( 0, outputId, m_surfaceTexcoordName );
		}

		public override void Destroy()
		{
			base.Destroy();
			m_referenceNode = null;
		}

	}
}
