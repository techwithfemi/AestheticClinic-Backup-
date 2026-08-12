export interface Employee {
  empId?: string;
  lastName: string;
  firstName: string;
  designationId?: string;
  designationName?: string;
  deptId?: string;
  deptName?: string;
  active: boolean;
  dob?: string | null;
  sex?: string;
  empStatusCode?: string;
}

export interface EmployeeReportRow {
  empId: string;
  fullname?: string;
  dept?: string;
  designation?: string;
  phone?: string;
  dob?: string | null;
  age?: number | null;
}

export interface Designation {
  designationId: string;
  designationName?: string;
  /** Number of employees currently assigned to this designation. Populated by the list endpoint. */
  inUseCount?: number;
}

export interface EmpDepartment {
  deptId: string;
  deptName?: string;
}

/** Full department shape used by the Department entry-form UI (mirrors `DepartmentVM`). */
export interface Department {
  deptId?: string;
  deptName?: string;
  deptAddress?: string;
  location?: string;
  /** Number of employees currently assigned to this department. Populated by the list endpoint. */
  inUseCount?: number;
}
