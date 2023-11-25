import { Typography } from 'antd'
import React from 'react'

export default function Profile() {
  const textStyle: React.CSSProperties = {
    fontSize: '19px',
    color: '#333',
    border: '0.005px solid #333',
    textAlign: 'center'
  }
  return (
    <div className='text-center border border-[#333] text-lg' style={textStyle}>
      <Typography.Text>Chấp Nhận Điều Khoản:</Typography.Text>
      <Typography.Text>
        a. Bằng cách truy cập và sử dụng trang web chúng tôi, bạn đồng ý tuân thủ tất cả các điều khoản và điều kiện
        được mô tả trong tài liệu này.
      </Typography.Text>
      Bản Quyền:
      <Typography.Text>
        a. Tất cả nội dung trên trang web này là tài sản của chúng tôi và được bảo vệ bởi luật bản quyền.
      </Typography.Text>
      <Typography.Text>
        b. Bạn có thể chia sẻ và tái sử dụng nội dung với điều kiện phải có sự đồng ý bằng văn bản từ chúng tôi và phải
        giữ nguyên thông tin về bản quyền.
      </Typography.Text>
      Nội Dung Người Dùng:
      <Typography.Text>
        a. Bất kỳ nội dung nào bạn đăng tải trên trang web phải tuân thủ các quy định về bản quyền và không được vi phạm
        quyền riêng tư, quyền của bên thứ ba, hoặc bất kỳ quy định pháp luật nào.
      </Typography.Text>
      Tạo Lập Liên Kết:
      <Typography.Text>
        a. Bạn có thể tạo liên kết đến trang web của chúng tôi, nhưng chúng tôi không chịu trách nhiệm về nội dung hoặc
        hành động của bất kỳ trang web nào mà liên kết đến.
      </Typography.Text>
      Chấm Dứt Truy Cập:
      <Typography.Text>
        a. Chúng tôi có quyền chấm dứt quyền truy cập của bạn vào trang web nếu chúng tôi cho rằng bạn đã vi phạm bất kỳ
        điều khoản nào trong tài liệu này.
      </Typography.Text>
      Miễn Trừ Trách Nhiệm:
      <Typography.Text>
        a. Chúng tôi không chịu trách nhiệm về bất kỳ tổn thất hoặc thiệt hại nào phát sinh từ việc sử dụng trang web
        của chúng tôi.
      </Typography.Text>
      Thay Đổi Điều Khoản:
      <Typography.Text>
        a. Chúng tôi có quyền thay đổi điều khoản này bất cứ lúc nào và sẽ công bố những thay đổi này trên trang web.
      </Typography.Text>
    </div>
  )
}
