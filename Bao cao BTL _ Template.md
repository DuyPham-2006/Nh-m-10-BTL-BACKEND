## THÀNH VIÊN NHÓM 
NHÓM TRƯỞNG : SV1: Phạm Minh Duy (MSV:1871020192)
              SV2: Đỗ Huy Anh Duy (MSV:1871020190)
              SV3: Lê Hoàng Nam (MSV:1871020417)
# BÁO CÁO BÀI TẬP LỚN

## Xây dựng Backend Quản lý Hệ thống Nghĩa trang Nhân dân

---

## PHẦN I: MỞ ĐẦU

### 1. Tên đề tài
**"Xây dựng Backend Quản lý Hệ thống Nghĩa trang Nhân dân"**

### 2. Tính cấp thiết của đề tài

## Công tác quản lý nghĩa trang tại các địa phương hiện nay thường đối mặt với nhiều bất cập:

**Vấn đề lưu trữ**: Dữ liệu mộ phần, người mất chủ yếu lưu trên sổ sách cũ, dễ thất lạc, hư hỏng theo thời gian.

**Vấn đề tra cứu**: Người thân khó tìm kiếm chính xác vị trí mộ phần trong các nghĩa trang diện tích lớn hoặc đã lâu đời.

**Vấn đề quy hoạch**: Khó theo dõi diện tích đất còn trống, dẫn đến tình trạng chồng chéo hoặc sử dụng đất không hiệu quả.

**Vấn đề dịch vụ**: Việc quản lý phí chăm sóc, bảo trì và các dịch vụ tâm linh đi kèm chưa được hệ thống hóa, thiếu minh bạch.

**Giải pháp**: Xây dựng hệ thống Backend quản lý tập trung nhằm số hóa hồ sơ mộ phần, sơ đồ hóa vị trí và tự động hóa quản lý tài chính dịch vụ.

### 3. Mục tiêu đề tài

Mục tiêu chính của bài tập lớn:

1. **Thiết kế cơ sở dữ liệu** tối ưu để lưu trữ thông tin mộ phần, thân nhân và dịch vụ.
2. **Xây dựng API Backend** hỗ trợ tìm kiếm vị trí mộ phần và quản lý trạng thái đất (trống/đã sử dụng).
3. **Triển khai logic quản lý phí** bảo trì định kỳ và các dịch vụ mai táng.
4. **Phát triển hệ thống báo cáo** về tỷ lệ lấp đầy và doanh thu dịch vụ.
5. **Đảm bảo tính bảo mật** thông tin cá nhân và lịch sử gia phả.

### 4. Phạm vi công việc

- **Phạm vi dữ liệu**: Quản lý các khu mộ, lô mộ, thông tin người mất và hợp đồng dịch vụ.
- **Phạm vi chức năng**: Cấp cho 3 nhóm người dùng (Nhân viên quản trang, Quản lý nghĩa trang, Admin).
- **Phạm vi công nghệ**: Backend RESTful API (Node.js/Express hoặc Python/Flask), MySQL/PostgreSQL.
- **Phạm vi thời gian**: 1 kỳ học (4-5 tháng)

---

## PHẦN II: PHÂN TÍCH YÊU CẦU HỆ THỐNG

### 1. Phân tích đối tượng sử dụng

| **Nhóm người dùng** | **Vai trò** | **Nhu cầu chính** |
|---|---|---|
| **Nhân viên quản trang** | Người vận hành trực tiếp | Cập nhật thông tin mộ phần, ghi nhận dịch vụ, thu phí bảo trì.|
| **Ban quản lý** | Người giám sát | Theo dõi quy hoạch đất, báo cáo doanh thu, phê duyệt hợp đồng. |
| **Quản trị Admin** | Cấu hình hệ thống | Quản lý danh mục khu vực, đơn giá dịch vụ và phân quyền tài khoản. |

**Kết luận**: Hệ thống tập trung vào **Backend quản lý nghiệp vụ**, hỗ trợ 3 loại người dùng với quyền hạn khác nhau.

### 2. Phân tích yêu cầu chức năng chính

#### **Nhóm chức năng A: Quản lý Quy hoạch & Mộ phần**
- Quản lý danh mục Khu (Area) và Lô (Plot).
- Cập nhật trạng thái mộ phần (Trống, Đã đặt chỗ, Đã sử dụng).
- Tìm kiếm mộ phần theo tên người mất, ngày mất hoặc vị trí.

#### **Nhóm chức năng B: Quản lý Hồ sơ & Hợp đồng**
- Lưu trữ hồ sơ người mất và thông tin thân nhân (Người liên hệ).
- Quản lý hợp đồng thuê đất/mua huyệt mộ.
- Quản lý hồ sơ cải táng/di dời (nếu có).

#### **Nhóm chức năng C: Vận hành & Báo cáo**
- Quản lý danh mục dịch vụ (Chăm sóc cây cảnh, thắp hương, tảo mộ).
- Theo dõi và thông báo phí bảo trì định kỳ.
- Lập báo cáo thống kê doanh thu và công nợ.

---

## PHẦN III: THIẾT KẾ CƠ SỞ DỮ LIỆU

### 1. Sơ đồ Lôgic ER (Entity-Relationship)

```
┌──────────────┐         ┌──────────────────┐
│    Area      │────────→│      Plot        │ (Mộ phần)
│  (Khu vực)   │   1:N   │                  │
│ PK: area_id  │         │ PK: plot_id      │
└──────────────┘         │ FK: area_id      │
                         └──────────────────┘
                                 │
                                 ├──→ ┌──────────────────┐
                                 │    │     Deceased     │ (Người mất)
                                 │    │                  │
                                 │    │ PK: deceased_id  │
                                 │    │ FK: plot_id      │
                                 │    └──────────────────┘
                                 │             │
                                 │    ┌──────────────────┐
                                 │    │     Relative     │ (Thân nhân)
                                 │    │                  │
                                 │    │ PK: relative_id  │
                                 │    │ FK: deceased_id  │
                                 └────└──────────────────┘
                                               │
          ┌────────────────┐          ┌──────────────────┐
          │    Service     │          │     Contract     │
          │  (Dịch vụ)     │←─────────│    (Hợp đồng)    │
          │ PK: service_id │   N:N    │ PK: contract_id  │
          └────────────────┘          └──────────────────┘
```

### 2. Chi tiết các bảng dữ liệu

#### **Bảng 1: Area (Khu vực nghĩa trang)**
```sql
CREATE TABLE Area (
    area_id INT PRIMARY KEY AUTO_INCREMENT,
    area_name VARCHAR(100) NOT NULL, 
    total_plots INT NOT NULL,
    description TEXT,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
);
```

---

#### **Bảng 2: Plot (Mộ phần/Lô đất)**
```sql
CREATE TABLE Plot (
    plot_id INT PRIMARY KEY AUTO_INCREMENT,
    area_id INT NOT NULL,
    plot_code VARCHAR(50) UNIQUE NOT NULL, 
    plot_type ENUM('SINGLE', 'DOUBLE', 'FAMILY') DEFAULT 'SINGLE',
    status ENUM('AVAILABLE', 'RESERVED', 'OCCUPIED', 'MAINTENANCE') DEFAULT 'AVAILABLE',
    price_base DECIMAL(15,2),
    FOREIGN KEY (area_id) REFERENCES Area(area_id),
    INDEX (status)
);
```

---

#### **Bảng 3: Deceased (Thông tin người mất)**
```sql
CREATE TABLE Deceased (
    deceased_id INT PRIMARY KEY AUTO_INCREMENT,
    plot_id INT UNIQUE, 
    full_name VARCHAR(100) NOT NULL,
    date_of_birth DATE,
    date_of_death DATE,
    burial_date DATE,
    hometown VARCHAR(255),
    bio_summary TEXT,
    FOREIGN KEY (plot_id) REFERENCES Plot(plot_id)
);
```

---

#### **Bảng 4: Relative (Thân nhân/Người liên hệ)**
```sql
CREATE TABLE Relative (
    relative_id INT PRIMARY KEY AUTO_INCREMENT,
    deceased_id INT NOT NULL,
    full_name VARCHAR(100) NOT NULL,
    relationship VARCHAR(50), 
    phone_number VARCHAR(15) NOT NULL,
    address VARCHAR(255),
    is_primary_contact BOOLEAN DEFAULT TRUE,
    FOREIGN KEY (deceased_id) REFERENCES Deceased(deceased_id)
);
```

---

#### **Bảng 5: Service (Danh mục dịch vụ)**
```sql
CREATE TABLE Service (
    service_id INT PRIMARY KEY AUTO_INCREMENT,
    service_name VARCHAR(100) NOT NULL,
    unit VARCHAR(50), 
    price DECIMAL(12,2) NOT NULL,
    is_periodic BOOLEAN DEFAULT FALSE 
);
```

---

#### **Bảng 6: Contract (Hợp đồng/Giao dịch)**
```sql
CREATE TABLE Contract (
    contract_id INT PRIMARY KEY AUTO_INCREMENT,
    plot_id INT NOT NULL,
    relative_id INT NOT NULL,
    contract_date DATE NOT NULL,
    total_amount DECIMAL(15,2) NOT NULL,
    payment_status ENUM('PENDING', 'PAID', 'PARTIAL') DEFAULT 'PENDING',
    expiry_date DATE, 
    FOREIGN KEY (plot_id) REFERENCES Plot(plot_id),
    FOREIGN KEY (relative_id) REFERENCES Relative(relative_id)
);
```

---

#### **Bảng 7: Service_Detail (Chi tiết dịch vụ trong hợp đồng)**
```sql
CREATE TABLE Service_Detail (
    detail_id INT PRIMARY KEY AUTO_INCREMENT,
    contract_id INT NOT NULL,
    service_id INT NOT NULL,
    quantity INT DEFAULT 1,
    subtotal DECIMAL(15,2) NOT NULL,
    FOREIGN KEY (contract_id) REFERENCES Contract(contract_id),
    FOREIGN KEY (service_id) REFERENCES Service(service_id)
);
```

---

#### **Bảng 8: User_Account (Quản lý hệ thống)**

```sql
CREATE TABLE User_Account (
    user_id INT PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(50) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    role ENUM('STAFF', 'MANAGER', 'ADMIN') DEFAULT 'STAFF',
    full_name VARCHAR(100),
    last_login DATETIME
);
```

---

### 3. Tóm tắt thiết kế dữ liệu

| **Bảng** | **Thành phần** | **Phụ trách** | **Chức năng** |
|---|---|---|---|
| **Cơ sở hạ tầng** | Area, Plot | SV1 | Quản lý sơ đồ, vị trí và trạng thái đất nghĩa trang. |
| **Hồ sơ nhân thân** | Deceased, Relative | SV1 | Lưu trữ thông tin người quá cố và thông tin liên lạc thân nhân. |
| **Kinh doanh** | Service, Contract, Detail | SV2 | Quản lý việc thuê đất, mua dịch vụ chăm sóc và thu phí. |
| **Hệ thống** | User_Account | SV3 |Bảo mật, phân quyền và lưu vết hoạt động.|

---

## PHẦN IV: THIẾT KẾ API BACKEND

### 1. Kiến trúc Backend (Monolithic/Architecture)

**Mô tả**: Backend được tổ chức theo mô hình **Monolithic** đơn giản, dễ triển khai cho đồ án sinh viên:

```
┌─────────────────────────────────────┐
│      Frontend (Web/Mobile)          │
└─────────────────────────────────────┘
                 │
                 ↓
┌─────────────────────────────────────┐
│      API Gateway / Authentication   │ (JWT/Session)
└─────────────────────────────────────┘
                 │
        ┌────────┼────────┐
        ↓        ↓        ↓
┌──────────────────────────────────────┐
│        API Routes / Endpoints        │
├──────────────────────────────────────┤
│ ├─ Area API (SV1)               │
│ ├─ Plot API (SV1)            │
│ ├─ Deceased API (SV1)             │
│ ├─ Relative API (SV1)          │
│ ├─ Service API (SV2)             │
│ ├─ Contract API (SV2)           │
│ ├─ Detail API (SV2)                │
│ └─ User_Account API (SV3)         │
└──────────────────────────────────────┘
                 │
        ┌────────┼────────┐
        ↓        ↓        ↓
┌──────────────────────────────────────┐
│      Business Logic Layer            │
├──────────────────────────────────────┤
│ ├─ Customer Service                 │
│ ├─ Meter Service                    │
│ ├─ Pricing Service                  │
│ ├─ Billing Service (TÓM LỤC)        │
│ ├─ Reading Service                  │
│ ├─ Supply Service                   │
│ ├─ Payment Service                  │
│ ├─ Report Service                   │
│ └─ Auth Service                     │
└──────────────────────────────────────┘
                 │
                 ↓
┌─────────────────────────────────────┐
│         Database Layer (MySQL)       │
│  (8 bảng dữ liệu + Views)           │
└─────────────────────────────────────┘
```

---

### 2. Danh sách API Endpoints

#### **A. Area & Plot Management (SV1)**

| **Method** | **Endpoint** | **Mô tả** | **Quyền hạn** |
|---|---|---|---|
| GET |  `/api/v1/areas`| Lấy danh sách các khu vực | STAFF, ADMIN |
| POST | `/api/v1/areas` | Tạo khu vực mới | ADMIN |
| GET | `/api/v1/plots` | Lấy danh sách mộ phần (có filter theo trạng thái/khu) | STAFF, ADMIN |
| GET | `/api/v1/plots/{id}` | Xem chi tiết vị trí mộ phần | STAFF, ADMIN |
| PUT | `/api/v1/plots/{id}/status` | Cập nhật trạng thái lô đất (Trống/Đã đặt) | STAFF, ADMIN |

---

#### **B. Deceased & Relative Management (SV2)**

| **Method** | **Endpoint** | **Mô tả** | **Quyền hạn** |
Method,Endpoint,Mô tả,Quyền hạn
GET,/api/v1/deceased,Tìm kiếm người mất (theo tên/ngày mất),"STAFF, ADMIN"
POST,/api/v1/deceased,Tiếp nhận hồ sơ người mất mới,STAFF
GET,/api/v1/deceased/{id},Xem hồ sơ chi tiết và vị trí mộ,"STAFF, ADMIN"
POST,/api/v1/relatives,Đăng ký thông tin thân nhân liên hệ,STAFF
GET,/api/v1/deceased/{id}/relatives,Lấy danh sách thân nhân của người mất,"STAFF, ADMIN"

---

#### **C. Service & Contract Management (SV3)**

| **Method** | **Endpoint** | **Mô tả** | **Quyền hạn** |
Method,Endpoint,Mô tả,Quyền hạn
GET,/api/v1/services,Danh mục dịch vụ và đơn giá,ALL
POST,/api/v1/contracts,Lập hợp đồng thuê đất/dịch vụ (Logic lõi),"STAFF, ADMIN"
GET,/api/v1/contracts/{id},Chi tiết hóa đơn và các dịch vụ đi kèm,"STAFF, ADMIN"
GET,/api/v1/reports/occupancy,Báo cáo tỷ lệ lấp đầy nghĩa trang,"MANAGER, ADMIN"
GET,/api/v1/reports/revenue,Báo cáo doanh thu dịch vụ theo tháng,"MANAGER, ADMIN"

---

### 3. Cấu trúc Request/Response mẫu

#### **Ví dụ: Lập hợp đồng dịch vụ (POST /api/v1/contracts)**

**Request Body**:
```json
{
  "plot_id": 105,
  "relative_id": 45,
  "services": [
    {"service_id": 1, "quantity": 1}, 
    {"service_id": 3, "quantity": 5} 
  ],
  "contract_date": "2026-03-15"
}
```

**Response Success (201)**:
```json
{
  "success": true,
  "message": "Hợp đồng đã được khởi tạo",
  "data": {
    "contract_id": 88,
    "total_amount": 15500000,
    "payment_status": "PENDING",
    "expiry_date": "2031-03-15"
  }
}
```

---

## PHẦN V: PHÂN CÔNG NHIỆM VỤ SINH VIÊN

### 1. Thống kê quy mô nhóm

- Số lượng: 3 sinh viên.

- Quy mô CSDL: 8 bảng chính.

- Công nghệ: Node.js/Python, MySQL, JWT cho bảo mật.
### 2. Bảng phân công chi tiết

#### **SV1: Quản lý Quy hoạch & Hạ tầng**

- Bảng dữ liệu: Area, Plot, User_Account.

## Nhiệm vụ API:

- Xây dựng toàn bộ CRUD cho Khu vực (Area) và Mộ phần (Plot).

- Xây dựng hệ thống Đăng nhập (Authentication) và Phân quyền (Authorization).

- Logic đặc biệt: Xử lý logic chuyển đổi trạng thái lô đất (khi khách đặt chỗ hoặc khi hết hạn hợp đồng).

Độ khó: ⭐⭐ (Trung bình)

---

#### **SV2: Quản lý Hồ sơ & Nhân thân**

- Bảng dữ liệu: Deceased, Relative.

## Nhiệm vụ API:

- Xây dựng công cụ tìm kiếm nâng cao (Search Engine) cho hồ sơ người mất.

- Quản lý thông tin đa thân nhân và người chịu trách nhiệm chính.

- Logic đặc biệt: Xây dựng thuật toán tìm kiếm theo âm tiết tiếng Việt và liên kết vị trí tọa độ lô đất.

Độ khó: ⭐⭐ (Trung bình)

---

#### **SV3: Quản lý Dịch vụ & Tài chính **

- Bảng dữ liệu: Service, Contract, Service_Detail.

## Nhiệm vụ API:

- Xây dựng logic tính toán tiền hợp đồng tự động dựa trên đơn giá dịch vụ.

- Xây dựng hệ thống báo cáo (Reporting) bằng SQL nâng cao (Aggregations).

- Logic đặc biệt: Quản lý gia hạn hợp đồng và tính toán phí bảo trì định kỳ (theo năm/gói).

Độ khó: ⭐⭐⭐ (Cao)

---

### 3. Bảng tóm tắt phân công

Tính năng:SV1,SV2,SV3
Thiết kế Database chung:R,R,A
Quản lý quy hoạch khu vực:R,I,I
Số hóa hồ sơ người mất:I,R,I
Tính toán hóa đơn dịch vụ:I,I,R
Báo cáo doanh thu & Lấp đầy:I,I,R

Ghi chú: R (Responsible - Thực hiện), A (Accountable - Chịu trách nhiệm chính), I (Informed - Tham khảo).

---

## PHẦN VI: CHI TIẾT LOGIC TÍNH TIỀN BẬC THANG (SV2)

### 1. Mô tả vấn đề

- Khác với tính hóa đơn nước theo chỉ số, hệ thống quản lý nghĩa trang tính tiền dựa trên loại đất và các gói dịch vụ đi kèm (chăm sóc định kỳ hoặc dịch vụ một lần).
## Ví dụ minh họa: 
Một khách hàng ký hợp đồng cho mộ phần tại Khu VIP:
- Giá thuê đất (Plot price): 15,000,000 VND (trả một lần).
- Dịch vụ chăm sóc mộ: 1,200,000 VND/năm (thu trước 5 năm).
- Dịch vụ lễ vật (một lần): 500,000 VND.
Tổng giá trị hợp đồng: 15,000,000 + (1,200,000 \times 5) + 500,000 = 21,500,000 VND.
### 2. Thuật toán tính tiền (Pseudocode)

function createContract(plotId, relativeId, serviceList, durationYears):
    // 1. Kiểm tra trạng thái lô đất
    plot = getPlotById(plotId)
    if plot.status != 'AVAILABLE':
        return Error("Lô đất đã được sử dụng hoặc đặt chỗ")

    // 2. Tính toán tiền đất
    totalAmount = plot.price_base
    
    // 3. Tính tiền các dịch vụ đi kèm
    for each item in serviceList:
        service = getServiceById(item.id)
        if service.is_periodic:
            // Phí định kỳ (như chăm sóc mộ)
            serviceAmount = service.price * durationYears
        else:
            // Dịch vụ một lần
            serviceAmount = service.price * item.quantity
        
        totalAmount += serviceAmount

    // 4. Lưu vào Database (Transaction)
    contract = saveToContractTable({
        plot_id: plotId,
        relative_id: relativeId,
        total_amount: totalAmount,
        expiry_date: calculateExpiry(durationYears)
    })
    
    // 5. Cập nhật trạng thái lô đất
    updatePlotStatus(plotId, 'RESERVED') // Hoặc 'OCCUPIED' nếu có hồ sơ người mất

    return contract

### 3. Code mẫu logic tính tiền(Node.js/Express)

```javascript
// File: services/contractService.js
class ContractService {
    /**
     * Tính toán tổng tiền hợp đồng và ngày hết hạn phí bảo trì
     */
    async calculateContractValue(plotId, services, durationYears, db) {
        // 1. Lấy giá đất cơ bản
        const [plots] = await db.query('SELECT price_base FROM Plot WHERE plot_id = ?', [plotId]);
        let totalAmount = parseFloat(plots[0].price_base);

        // 2. Lấy danh sách giá dịch vụ hiện hành
        const [availableServices] = await db.query('SELECT * FROM Service');
        
        const contractDetails = [];
        
        for (const item of services) {
            const service = availableServices.find(s => s.service_id === item.service_id);
            if (!service) continue;

            let cost = 0;
            if (service.is_periodic) {
                // Phí định kỳ tính theo năm (ví dụ: phí quản lý nghĩa trang)
                cost = service.price * durationYears;
            } else {
                cost = service.price * (item.quantity || 1);
            }

            totalAmount += cost;
            contractDetails.push({
                service_id: service.service_id,
                subtotal: cost
            });
        }

        // 3. Tính ngày hết hạn (ví dụ: 5 năm sau ngày ký)
        const expiryDate = new Date();
        expiryDate.setFullYear(expiryDate.getFullYear() + durationYears);

        return { totalAmount, expiryDate, contractDetails };
    }
}

module.exports = new ContractService();
```

---

## PHẦN VII: YÊU CẦU KỸ THUẬT & MÔI TRƯỜNG LÀM VIỆC

### 1. Stack công nghệ

| **Lớp** | **Công nghệ** | **Phiên bản** |
|---|---|---|
| **Backend** | Node.js + Express | v18+ |
| **Database** | MySQL | v5.7+ hoặc v8.0 |
| **Authentication** | JWT (JSON Web Token) | - |
| **Password Hash** | Bcrypt | - |
| **Validation** | Joi | - |
| **API Documentation** | Swagger/OpenAPI | v3.0 |
| **Version Control** | Git + GitHub | - |

### 2. Cấu trúc thư mục Backend

```
cemetery-backend/
├── src/
│   ├── config/          # Kết nối Database, cấu hình JWT, biến môi trường
│   ├── controllers/      # Tiếp nhận Request, điều hướng logic và trả về Response
│   ├── services/         # Xử lý logic nghiệp vụ (Tính phí hợp đồng, báo cáo lấp đầy)
│   ├── models/           # Định nghĩa các câu lệnh SQL/Schemas truy vấn dữ liệu
│   ├── middleware/       # Kiểm tra quyền (Auth), xử lý lỗi, Log hệ thống
│   ├── routes/           # Định nghĩa các đường dẫn API (/api/v1/plots, /api/v1/deceased)
│   ├── validators/       # Ràng buộc định dạng dữ liệu (Tên không trống, ngày chết hợp lệ)
│   └── app.js            # Khởi tạo Express app và tích hợp các middleware
├── tests/                # Chứa các bản kiểm thử tự động (Unit test, Integration test)
├── db/
│   ├── schema.sql        # File khởi tạo cấu trúc 8 bảng dữ liệu
│   └── seed.sql          # Dữ liệu mẫu để chạy thử hệ thống
├── docs/                 # Tài liệu thiết kế hệ thống và API Swagger
├── .env                  # Lưu trữ mã bí mật (Secret Key, DB Password)
├── .gitignore            # Khai báo các file không đưa lên GitHub
├── package.json          # Quản lý các thư viện (dependencies) của dự án
└── server.js             # File thực thi chính để khởi chạy Server
```

### 3. Yêu cầu môi trường

**Máy tính cá nhân:**
- Runtime: Node.js v18.x trở lên để hỗ trợ các cú pháp JavaScript mới nhất.

- Database: MySQL Community Server 8.0 hoặc MariaDB.

- IDE: Visual Studio Code (Cài thêm các Extension: ESLint, Prettier, MySQL Client).

* Công cụ Test:

- Postman: Dùng để giả lập các Request gửi đến Backend.

- Thunder Client: Extension tích hợp sẵn trong VS Code để test nhanh API.

- Git: Cần thiết để các thành viên (SV1, SV2, SV3) gộp code (Merge) mà không bị xung đột.

**Máy chủ (Deployment - tùy chọn):**
- Cloud Hosting: Có thể sử dụng Render hoặc Railway (Miễn phí/Giá rẻ cho sinh viên) để chạy Node.js Server.

- Managed Database: Sử dụng Aiven hoặc PlanetScale để lưu trữ cơ sở dữ liệu MySQL trên đám mây.

- CI/CD: Tích hợp GitHub Actions để tự động kiểm tra lỗi mỗi khi có thành viên đẩy code mới lên.

---



## PHẦN VIII: LỊCH TRÌNH THỰC HIỆN

| **Tuần** | **Mốc quan trọng** | **Yêu cầu** |
|---|---|---|
| 1-2 | Khởi tạo & CSDL | Thiết kế ERD; Cài đặt Server Node.js; Tạo Schema SQL cho 8 bảng chính |
| 3-4 | SV1: Phát triển Core API | Xây dựng CRUD cho Area, Plot; Quản lý hồ sơ người mất (Deceased) |
| 5-6 | SV2: Nghiệp vụ Hợp đồng | Logic tạo Contract; Tính tiền dịch vụ & phí bảo trì; Quản lý Relative |
| 7-8 | SV3: Vận hành & Báo cáo | API thanh toán; Logic thống kê doanh thu; Báo cáo tỷ lệ lấp đầy |
| 9 | Tích hợp & Kiểm thử | Tích hợp JWT; Test luồng nghiệp vụ (End-to-End); Sửa lỗi (Fix bugs) |
| 10 | Đóng gói & Demo | Hoàn thiện tài liệu báo cáo; Chuẩn bị Slide & Video Demo sản phẩm |

---

## KẾT LUẬN

Bài tập lớn **"Xây dựng Backend Quản lý Nghĩa trang nhân dân"** là một đề tài thực tế, phù hợp với năng lực sinh viên năm hai. 

1. Kết quả đạt được
- Sau quá trình nghiên cứu, phân tích và triển khai hệ thống Backend Quản lý Nghĩa trang Nhân dân, nhóm đã hoàn thành các mục tiêu đề ra ban đầu:

- Về mặt dữ liệu: Thiết kế thành công cấu trúc cơ sở dữ liệu quan hệ gồm 8 bảng tối ưu, đảm bảo tính toàn vẹn và khả năng mở rộng để quản lý hàng nghìn mộ phần và hồ sơ người mất.

- Về mặt chức năng: Xây dựng hệ thống API RESTful đầy đủ cho 3 nhóm người dùng. Đặc biệt đã giải quyết tốt bài toán quản lý quỹ đất trống và logic tính phí hợp đồng phức tạp.

- Về mặt kỹ thuật: Áp dụng thành công các công nghệ hiện đại như Node.js, JWT cho bảo mật và Bcrypt cho mã hóa dữ liệu, giúp hệ thống vận hành ổn định và an toàn.

- Về mặt quản lý: Cung cấp công cụ báo cáo trực quan về doanh thu và tỷ lệ lấp đầy, hỗ trợ đắc lực cho Ban quản lý trong việc đưa ra các quyết định quy hoạch.

2. Ưu điểm của hệ thống
- Tính minh bạch: Quy trình lập hợp đồng và tính phí dịch vụ được tự động hóa, loại bỏ các sai sót do tính toán thủ công bằng sổ sách.

- Khả năng tra cứu: Hệ thống cho phép tìm kiếm người mất theo nhiều tiêu chí (tên, ngày mất, khu vực) với tốc độ phản hồi nhanh.

- Phân quyền chặt chẽ: Đảm bảo nhân viên chỉ tiếp cận đúng chức năng được giao, bảo vệ thông tin nhạy cảm của khách hàng.

- Dễ dàng bảo trì: Cấu trúc thư mục tách biệt giữa Controller và Service giúp việc nâng cấp tính năng trong tương lai trở nên đơn giản.

3. Hạn chế và Hướng phát triển
Hạn chế:

- Hệ thống hiện tại mới chỉ dừng lại ở mức giao diện API Backend, chưa có giao diện người dùng (Frontend) hoàn chỉnh.

- Chưa tích hợp sơ đồ mặt bằng trực quan (GIS) để theo dõi vị trí mộ phần trên bản đồ 2D/3D.

* Hướng phát triển:

- Tích hợp QR Code: Mỗi mộ phần sẽ có một mã QR riêng để thân nhân dễ dàng truy cập hồ sơ tiểu sử và thanh toán phí chăm sóc qua ví điện tử.

- Số hóa bản đồ: Sử dụng các thư viện như Leaflet hoặc Mapbox để xây dựng sơ đồ nghĩa trang số, giúp nhân viên quản trang theo dõi vị trí mộ phần trực quan hơn.


# CẢM ƠN THẦY VÀ CÁC BẠN ĐÃ THEO DÕI BÁO CÁO CỦA NHÓM!