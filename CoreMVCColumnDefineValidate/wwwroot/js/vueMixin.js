const mixin = {
	data() {
		return {
			ColumnDefines: {}
		}
	}, 
    methods: {
        // 產生控制物件
        CreateFormControl(form) {
			for (idx in form.ColumnList) {
				// 欄位控制物件
				let control = {
					id: form.name + '.' + form.ColumnList[idx],
					value: '',
					name: '',
					format: '',
					maxLength: '',
					minLength: '',
					rangeStart: '',
					rangeEnd: '',
					required: false
				}
				form[form.ColumnList[idx]] = control;
			}
		}
		// 綁定欄位定義
		, BindColumnDefine(form) {
			var self = this;
			for (idx in form.ColumnList) {
				let dd = $.grep(self.ColumnDefines, function (o) { return o.ColumnID === form.ColumnList[idx] });
				if (dd.length > 0) {
					let control = form[form.ColumnList[idx]];
					control.name = dd[0].ColumnName;// 欄位名稱
					control.format = dd[0].ColumnFormat;// 欄位格式
					control.maxLength = dd[0].ColumnMaxLength;// 最大長度
					control.minLength = dd[0].ColumnMinLength;// 最小長度
					control.rangeStart = dd[0].ColumnRangeStart;// 數字最小範圍
					control.rangeEnd = dd[0].ColumnRangeEnd;// 數字最大範圍
				}
			}
		}
		// 檢查資料定義
		, Validate(form, id) {
			if (form[id].value.length > 0) {
				let msg = '';
				let name = '';
				if (form[id].name) {
					name = "[" + form[id].name + "] ";
				}
				if (form[id].format === 'INT') {
					let re = /[^0-9]/;
					if (re.test(form[id].value)) {
						msg = "整數格式錯誤";
					}
				} else if (form[id].format === 'NUM') {
					if (isNaN(form[id].value)) {
						msg = "數字格式錯誤";
					}
				} else if (form[id].format === 'DATE') {
					var dateStr = form[id].value.replace(/\/0+/g, '/');

					var accDate = new Date(dateStr);
					var tempDate = accDate.getFullYear() + "/";
					tempDate += (accDate.getMonth() + 1) + "/";
					tempDate += accDate.getDate();

					if (dateStr !== tempDate) {
						msg = "日期格式錯誤";
					}
				} else if (form[id].format === 'PID') {
					let check = true;
					if (form[id].value.length !== 10) {
						check = false;
					}
					if (check) {
						tab = "ABCDEFGHJKLMNPQRSTUVXYWZIO";
						A1 = new Array(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3);
						A2 = new Array(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5);
						Mx = new Array(9, 8, 7, 6, 5, 4, 3, 2, 1, 1);

						if (form[id].value.length !== 10) {
							check = false;
						}
						i = tab.indexOf(form[id].value.charAt(0));
						if (i === -1) {
							check = false;
						}
						sum = A1[i] + A2[i] * 9;

						for (i = 1; i < 10; i++) {
							v = parseInt(form[id].value.charAt(i));
							if (isNaN(v)) {
								check = false;
							}
							sum = sum + v * Mx[i];
						}
						if (sum % 10 !== 0) {
							check = false;
						}
					}


					if (!check) {
						msg = '身份證字號格式錯誤';
					}
				} else if (form[id].format === 'EMAIL') {
					if (form[id].value.indexOf("@") === -1 || form[id].value.indexOf(".") === -1 || form[id].value.split('@').length > 2 || form[id].value.length <= 4) {
						msg = 'E-Mail 格式錯誤';
					}
				} else if (form[id].format === 'PHONE') {
					let regExp = /^[\s()+-]*([0-9][\s()+-]*){6,20}$/;
					if (form[id].value.match(regExp) === null) {
						msg = '電話格式錯誤';
					}
				}

				if (msg == '') {
					//長度檢查
					if (form[id].maxLength != '') {
						if (form[id].value.length > form[id].maxLength) {
							msg = '長度最多 ' + form[id].maxLength + ' 字元';
						}
					}
					if (form[id].minLength != '') {
						if (form[id].value.length < form[id].minLength) {
							msg = '長度最少 ' + form[id].maxLength + ' 字元';
						}
					}
				}

				if (msg == '') {
					//數字範圍檢查
					if (form[id].rangeStart != '') {
						if (parseFloat(form[id].value) < parseFloat(form[id].rangeStart)) {
							msg = '數字範圍最小為 ' + form[id].rangeStart;
						}
					}
					if (form[id].rangeEnd != '') {
						if (parseFloat(form[id].value) > parseFloat(form[id].rangeEnd)) {
							msg = '數字範圍最大為 ' + form[id].rangeEnd;
						}
					}
				}

				if (msg !== '') {
					alert(name + msg);
					form[id].value = '';
					this.$refs[form[id].id].focus();
				}
			}
		}
		// 檢查是否必填
		, CheckRequired(form) {
			let msg = '';
			for (idx in form) {
				if (form[idx].required) {
					if (form[idx].value == '') {
						let name = '';
						if (form[idx].name) {
							name = "[" + form[idx].name + "] ";
						}
						msg += name + "欄位不可空白\n";
					}
				}
			}
			return msg;
		}
    }
}