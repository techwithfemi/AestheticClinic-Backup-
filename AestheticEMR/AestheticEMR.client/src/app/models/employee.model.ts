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
