import { Typography } from 'antd'
import React from 'react'

export default function Profile() {
  const textStyle: React.CSSProperties = {
    
    color: '#333',
    border: '0.005px solid #f5f5f5',
    textAlign: 'left',
    paddingLeft: '7px',
    paddingTop: '3px',
    paddingBottom: '3px',
  }
  return (
    <div className='text-center border border-[#333] text-lg' style={textStyle}>
      <Typography.Text> Users:</Typography.Text><br/>
      <Typography.Text>
        - Users has over 1000 followers will be rewarded.<br/>
        - Users can posts in any subjects and any majors they want.<br/>
        - Users can report other users equally.<br/>
        - Anyone reaches 5 approved reports will be banned by the Admin.<br/>
      </Typography.Text><br/>
      <Typography.Text>Posts:</Typography.Text><br/>
      <Typography.Text>
        - Posts must not content any sensitive words or contents.<br/>
        - Posts must support academic purposes.<br/>
        - Posts must be relative to the subjects and majors that it is tagged.<br/>
      </Typography.Text><br/>
    </div>
  )
}
