import React from 'react';


export default function Profile() {
  const textStyle: React.CSSProperties = {
    fontSize: '20px', 
    color: '#333', 
    border: '0.1px solid #333', 
    
  };
  return (
    <div style={textStyle}>
    <p>Chấp Nhận Điều Khoản:
    <div>a. Bằng cách truy cập và sử dụng trang web chúng tôi, bạn đồng ý tuân thủ tất cả các điều khoản và điều kiện được mô tả trong tài liệu này.</div>

    Bản Quyền:
    <div>a. Tất cả nội dung trên trang web này là tài sản của chúng tôi và được bảo vệ bởi luật bản quyền.</div>
    <div>b. Bạn có thể chia sẻ và tái sử dụng nội dung với điều kiện phải có sự đồng ý bằng văn bản từ chúng tôi và phải giữ nguyên thông tin về bản quyền.</div>
    Nội Dung Người Dùng:
    <div>a. Bất kỳ nội dung nào bạn đăng tải trên trang web phải tuân thủ các quy định về bản quyền và không được vi phạm quyền riêng tư, quyền của bên thứ ba, hoặc bất kỳ quy định pháp luật nào.</div>
    Tạo Lập Liên Kết:
    <div>a. Bạn có thể tạo liên kết đến trang web của chúng tôi, nhưng chúng tôi không chịu trách nhiệm về nội dung hoặc hành động của bất kỳ trang web nào mà liên kết đến.</div>
    Chấm Dứt Truy Cập:
    <div>a. Chúng tôi có quyền chấm dứt quyền truy cập của bạn vào trang web nếu chúng tôi cho rằng bạn đã vi phạm bất kỳ điều khoản nào trong tài liệu này.</div>
    Miễn Trừ Trách Nhiệm:
    <div>a. Chúng tôi không chịu trách nhiệm về bất kỳ tổn thất hoặc thiệt hại nào phát sinh từ việc sử dụng trang web của chúng tôi.</div>
    Thay Đổi Điều Khoản:
    <div>a. Chúng tôi có quyền thay đổi điều khoản này bất cứ lúc nào và sẽ công bố những thay đổi này trên trang web.</div>
    </p>
  </div>
  )
}
