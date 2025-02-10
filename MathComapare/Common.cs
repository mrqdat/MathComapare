namespace MathComapare
{
    enum messageType
    {
        register_user = 0,          //Client -> Server    Gửi thông tin đăng ký người dùng
        register_user_ack = 1,      //Server -> Client    Xác nhận đăng ký thành công
        submit_score = 2,           //Client -> Server    Gửi điểm của người chơi
        score_update = 3,           //Server -> Client    Cập nhật điểm thời gian thực
        get_scores = 4,             //Client -> Server    Yêu cầu danh sách điểm
        get_scores_response = 5,    //Server -> Client    Trả về danh sách điểm
        error = -1,                  //Server -> Client    Phản hồi lỗi (nếu có)
    }
}
